using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Streaming;
using System.IO;
using System.Xml;
using BREPipelineFramework.Helpers.Tracing;

namespace BREPipelineFramework.Helpers
{
    public class MessageModificationTranslatorStream : XmlTranslatorStream
    {
        private List<MessageModificationDetails> messageModificationInstructions;
        private int hierarchyLevel = 0;
        private string lastNodeName = string.Empty;
        private string lastNodeNamespace = string.Empty;
        private bool skipWhiteSpace = false;
        private string callToken;
        private Stream inputStream;
        
        public MessageModificationTranslatorStream(Stream stream, List<MessageModificationDetails> messageModificationInstructions, string callToken)
            : base(XmlReader.Create(stream))
        {
            this.messageModificationInstructions = messageModificationInstructions;
            this.callToken = callToken;
            this.inputStream = stream;
        }

        protected override void TranslateElement()
        {
            skipWhiteSpace = false;
            bool skipElementTranslation = false;
            lastNodeName = m_reader.LocalName;
            lastNodeNamespace = m_reader.NamespaceURI;

            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.RemoveElement:
                    {
                        if (messageModificationInstruction.Name != null)
                        {
                            if (messageModificationInstruction.Name != m_reader.LocalName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.Namespace != null)
                        {
                            if (messageModificationInstruction.Namespace != m_reader.NamespaceURI)
                            {
                                break;
                            }
                        }

                        skipElementTranslation = true;
                        break;
                    }
                }
            }

            if (!skipElementTranslation)
            {
                base.TranslateElement();
            }
        }

        protected override void TranslateStartElement(string prefix, string localName, string nsURI)
        {
            bool rootNode = false;
            lastNodeName = m_reader.LocalName;
            lastNodeNamespace = m_reader.NamespaceURI;

            if (hierarchyLevel++ == 0)
            {
                rootNode = true;
                TraceManager.PipelineComponent.TraceInfo("{0} - Applying MessagemodificationTranslatorStream now", callToken);
            }

            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.AddRootNodeNamespaceAndPrefix:
                    {
                        if (rootNode)
                        {
                            if (string.IsNullOrEmpty(nsURI))
                            {
                                prefix = messageModificationInstruction.Prefix;
                                nsURI = messageModificationInstruction.Namespace;
                            }
                        }

                        break;
                    }
                    case MessageModificationInstructionTypeEnum.UpdateRootNodeNamespaceAndPrefix:
                    {
                        if (rootNode)
                        {
                            if (string.IsNullOrEmpty(prefix) || prefix != messageModificationInstruction.Prefix)
                            {
                                prefix = messageModificationInstruction.Prefix;
                            }
                            else
                            {
                                prefix = "prefixClash";
                            }

                            nsURI = messageModificationInstruction.Namespace;
                        }

                        break;
                    }   
                    case MessageModificationInstructionTypeEnum.UpdateNamespaceAndPrefix:
                    {
                        if (nsURI == messageModificationInstruction.OldNamespace)
                        {
                            if (!string.IsNullOrEmpty(messageModificationInstruction.Prefix))
                            {
                                prefix = messageModificationInstruction.Prefix;
                            }

                            if (!string.IsNullOrEmpty(messageModificationInstruction.Namespace))
                            {
                                nsURI = messageModificationInstruction.Namespace;
                            }
                        }

                        break;
                    }
                    case MessageModificationInstructionTypeEnum.RemoveNamespace:
                    {
                        if (nsURI == messageModificationInstruction.Namespace)
                        {
                            prefix = null;
                            nsURI = null;
                        }

                        break;
                    }
                    case MessageModificationInstructionTypeEnum.UpdateElementName:
                    {
                        if (messageModificationInstruction.OldName != null)
                        {
                            if (messageModificationInstruction.OldName != localName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.OldNamespace != null)
                        {
                            if (messageModificationInstruction.OldNamespace != nsURI)
                            {
                                break;
                            }
                        }

                        localName = messageModificationInstruction.Name;
                        break;
                    }
                }
            }

            base.TranslateStartElement(prefix, localName, nsURI);
        }

        protected override void TranslateText(string value)
        {
            bool skipTranslateText = false;

            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.UpdateElementValue:
                    {
                        if (messageModificationInstruction.Name != null)
                        {
                            if (messageModificationInstruction.Name != lastNodeName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.Namespace != null)
                        {
                            if (messageModificationInstruction.Namespace != lastNodeNamespace)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.OldValue != null)
                        {
                            if (messageModificationInstruction.OldValue != m_reader.Value)
                            {
                                break;
                            }
                        }

                        value = messageModificationInstruction.Value;
                        break;
                    }
                    case MessageModificationInstructionTypeEnum.RemoveElement:
                    {
                        TraceManager.PipelineComponent.TraceInfo(lastNodeName + " - " + lastNodeNamespace);

                        if (messageModificationInstruction.Name != null)
                        {
                            if (messageModificationInstruction.Name != lastNodeName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.Namespace != null)
                        {
                            if (messageModificationInstruction.Namespace != lastNodeNamespace)
                            {
                                break;
                            }
                        }

                        skipTranslateText = true;
                        break;
                    }
                }
            }

            if (!skipTranslateText)
            {
                base.TranslateText(value);
            }
        }

        protected override void TranslateEndElement(bool full)
        {
            skipWhiteSpace = false;
            bool skipElementTranslation = false;

            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.RemoveElement:
                    {
                        if (messageModificationInstruction.Name != null)
                        {
                            if (messageModificationInstruction.Name != m_reader.LocalName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.Namespace != null)
                        {
                            if (messageModificationInstruction.Namespace != m_reader.NamespaceURI)
                            {
                                break;
                            }
                        }

                        skipElementTranslation = true;
                        break;
                    }
                }
            }

            if (!skipElementTranslation)
            {
                base.TranslateEndElement(full);
            }
            else
            {
                skipWhiteSpace = true;
            }
        }

        protected override void TranslateWhitespace(string space)
        {
            if (!skipWhiteSpace)
            {
                base.TranslateWhitespace(space);
            }
        }

        protected override void TranslateAttribute()
        {
            bool skipAttributeTranslation = false;

            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.RemoveNamespace:
                    {
                        if (m_reader.LocalName == "xmlns" && m_reader.NamespaceURI == "http://www.w3.org/2000/xmlns/" && m_reader.Value == messageModificationInstruction.Namespace)
                        {
                            skipAttributeTranslation = true;
                        }
                        else if (m_reader.Prefix == "xmlns" && m_reader.NamespaceURI == "http://www.w3.org/2000/xmlns/" && m_reader.Value == messageModificationInstruction.Namespace)
                        {
                            skipAttributeTranslation = true;
                        }

                        break;
                    }
                    case MessageModificationInstructionTypeEnum.RemoveAttribute:
                    {
                        if (messageModificationInstruction.Name != null)
                        {
                            if (messageModificationInstruction.Name != m_reader.LocalName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.Namespace != null)
                        {
                            if (messageModificationInstruction.Namespace != m_reader.NamespaceURI)
                            {
                                break;
                            }
                        }

                        skipAttributeTranslation = true;
                        break;
                    }
                }
            }

            if (!skipAttributeTranslation)
            {
                base.TranslateAttribute();
            }
        }

        protected override void TranslateStartAttribute(string prefix, string localName, string nsURI)
        {
            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.UpdateNamespaceAndPrefix:
                    {
                        if (((localName == "xmlns") && (nsURI == "http://www.w3.org/2000/xmlns/")) && (m_reader.Value == messageModificationInstruction.OldNamespace))
                        {
                            if (!string.IsNullOrEmpty(messageModificationInstruction.Prefix))
                            {
                                prefix = "xmlns";
                                localName = messageModificationInstruction.Prefix;
                            }
                        }
                        else if (((prefix == "xmlns") && (nsURI == "http://www.w3.org/2000/xmlns/")) && (m_reader.Value == messageModificationInstruction.OldNamespace))
                        {
                            if (!string.IsNullOrEmpty(messageModificationInstruction.Prefix))
                            {
                                localName = messageModificationInstruction.Prefix;
                            }
                        }
                        else if (nsURI == messageModificationInstruction.OldNamespace)
                        {
                            if (!string.IsNullOrEmpty(messageModificationInstruction.Prefix))
                            {
                                prefix = messageModificationInstruction.Prefix;
                            }

                            if (!string.IsNullOrEmpty(messageModificationInstruction.Namespace))
                            {
                                nsURI = messageModificationInstruction.Namespace;
                            }
                        }

                        break;
                    }
                    case MessageModificationInstructionTypeEnum.UpdateAttributeName:
                    {
                        if (messageModificationInstruction.OldName != null)
                        {
                            if (messageModificationInstruction.OldName != localName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.OldNamespace != null)
                        {
                            if (messageModificationInstruction.OldNamespace != nsURI)
                            {
                                break;
                            }
                        }

                        localName = messageModificationInstruction.Name;
                        break;
                    }

                    case MessageModificationInstructionTypeEnum.RemoveNamespace:
                    {
                        if (nsURI == messageModificationInstruction.Namespace)
                        {
                            prefix = null;
                            nsURI = null;
                        }

                        break;
                    }
                }
            }

            base.TranslateStartAttribute(prefix, localName, nsURI);
        }

        protected override void TranslateAttributeValue(string prefix, string localName, string nsURI, string val)
        {
            foreach (MessageModificationDetails messageModificationInstruction in messageModificationInstructions)
            {
                switch (messageModificationInstruction.ModificationInstruction)
                {
                    case MessageModificationInstructionTypeEnum.UpdateNamespaceAndPrefix:
                    {
                        if (((localName == "xmlns") && (nsURI == "http://www.w3.org/2000/xmlns/")) && (val == messageModificationInstruction.OldNamespace))
                        {
                            if (!string.IsNullOrEmpty(messageModificationInstruction.Namespace))
                            {
                                val = messageModificationInstruction.Namespace;
                            }
                        }
                        else if (((prefix == "xmlns") && (nsURI == "http://www.w3.org/2000/xmlns/")) && (val == messageModificationInstruction.OldNamespace))
                        {
                            if (!string.IsNullOrEmpty(messageModificationInstruction.Namespace))
                            {
                                val = messageModificationInstruction.Namespace;
                            }
                        }

                        break;
                    }
                    case MessageModificationInstructionTypeEnum.UpdateAttributeValue:
                    {
                        if (messageModificationInstruction.OldName != null)
                        {
                            if (messageModificationInstruction.OldName != localName)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.OldNamespace != null)
                        {
                            if (messageModificationInstruction.OldNamespace != nsURI)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.OldValue != null)
                        {
                            if (messageModificationInstruction.OldValue != val)
                            {
                                break;
                            }
                        }

                        if (messageModificationInstruction.Value != null)
                        {
                            val = messageModificationInstruction.Value;
                        }

                        break;
                    }
                }
            }

            base.TranslateAttributeValue(prefix, localName, nsURI, val);
        }
    }
}