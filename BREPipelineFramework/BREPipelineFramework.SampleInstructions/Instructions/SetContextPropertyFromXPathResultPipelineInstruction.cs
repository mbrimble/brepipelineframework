using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using Microsoft.BizTalk.Streaming;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.XPath;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class SetContextPropertyFromXPathResultPipelineInstruction : IBREPipelineInstruction
    {
        private List<XPathInstruction> _XPathInstructions = new List<XPathInstruction>();

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            XmlTextReader xmlTextReader = new XmlTextReader(inmsg.BodyPart.GetOriginalDataStream());
            XPathCollection xPathCollection = GetXPathCollection();
            XPathReader xPathReader = new XPathReader(xmlTextReader, xPathCollection);

            while (xPathReader.ReadUntilMatch())
            {
                for (int i = 0; i < xPathCollection.Count; i++)
                {
                    if (xPathReader.Match(i))
                    {
                        string value = null;

                        _XPathInstructions.ElementAt(i).IsFound = true;
                        switch (_XPathInstructions.ElementAt(i).XPathResultType)
                        {
                            case XPathResultTypeEnum.Name:
                                value = xPathReader.LocalName;
                                break;
                            case XPathResultTypeEnum.Namespace:
                                value = xPathReader.NamespaceURI;
                                break;
                            case XPathResultTypeEnum.Value:
                                value = xPathReader.ReadString();
                                break;
                        }

                        if (_XPathInstructions.ElementAt(i).Promotion == ContextInstructionTypeEnum.Promote)
                        {
                            inmsg.Context.Promote(_XPathInstructions.ElementAt(i).PropertyName, _XPathInstructions.ElementAt(i).PropertyNamespace, TypeCaster.GetTypedObject(value, _XPathInstructions.ElementAt(i).Type));
                        }
                        else
                        {
                            inmsg.Context.Write(_XPathInstructions.ElementAt(i).PropertyName, _XPathInstructions.ElementAt(i).PropertyNamespace, TypeCaster.GetTypedObject(value, _XPathInstructions.ElementAt(i).Type));
                        }
                    }                
                }
            }

            for (int i = 0; i < xPathCollection.Count; i++)
            {
                if (_XPathInstructions.ElementAt(i).IsFound == false && _XPathInstructions.ElementAt(i).ExceptionIfNotFound == true)
                {
                    throw new Exception("Unable to evaluate XPath expression " + _XPathInstructions.ElementAt(i).XPathQuery + " against the message.");
                }
            }

            inmsg.BodyPart.Data.Position = 0;
        }

        public void AddXPathInstruction(XPathInstruction _XPathInstruction)
        {
            _XPathInstructions.Add(_XPathInstruction);
        }

        private XPathCollection GetXPathCollection()
        {
            XPathCollection _XPathCollection = new XPathCollection();

            foreach (var _XPathInstruction in _XPathInstructions)
            {
                _XPathCollection.Add(_XPathInstruction.XPathQuery);
            }

            return _XPathCollection;
        }
    }
}
