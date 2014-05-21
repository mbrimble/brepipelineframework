using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
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
            IBaseMessagePart bodyPart = inmsg.BodyPart;
            Stream inboundStream = bodyPart.GetOriginalDataStream();

            VirtualStream virtualStream = new VirtualStream();
            ReadOnlySeekableStream readOnlySeekableStream = new ReadOnlySeekableStream(inboundStream, virtualStream);
            XmlTextReader xmlTextReader = new XmlTextReader(readOnlySeekableStream);

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
                            default:
                                throw new Exception(string.Format("Unexpected xpath result type of {0} encountered", _XPathInstructions.ElementAt(i).XPathResultType.ToString()));
                        }

                        try
                        {
                            if (_XPathInstructions.ElementAt(i).Promotion == ContextInstructionTypeEnum.Promote)
                            {
                                inmsg.Context.Promote(_XPathInstructions.ElementAt(i).PropertyName, _XPathInstructions.ElementAt(i).PropertyNamespace, TypeCaster.GetTypedObject(value, _XPathInstructions.ElementAt(i).Type));
                            }
                            else
                            {
                                inmsg.Context.Write(_XPathInstructions.ElementAt(i).PropertyName, _XPathInstructions.ElementAt(i).PropertyNamespace, TypeCaster.GetTypedObject(value, _XPathInstructions.ElementAt(i).Type));
                            }
                        }
                        catch (Exception e)
                        {
                            if (_XPathInstructions.ElementAt(i).ExceptionIfNotFound == true)
                            {
                                throw new Exception("Unable to set context property " + _XPathInstructions.ElementAt(i).PropertyNamespace + "#" + _XPathInstructions.ElementAt(i).PropertyName + " from XPath Query application " + _XPathInstructions.ElementAt(i).XPathQuery + ". Encountered error - " + e.ToString());
                            }
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

            readOnlySeekableStream.Position = 0;
            bodyPart.Data = readOnlySeekableStream;
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
