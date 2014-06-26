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
    public class SetMessageTypeContextProperty : IBREPipelineInstruction
    {
        private List<XPathInstruction> _XPathInstructions = new List<XPathInstruction>();

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            string _XPathQuery = "/*";
            string rootNodeName = string.Empty;
            string rootNodeNamespace = string.Empty;
            
            XmlTextReader xmlTextReader = new XmlTextReader(inmsg.BodyPart.GetOriginalDataStream());
            XPathCollection xPathCollection = new XPathCollection();
            xPathCollection.Add(_XPathQuery);

            XPathReader xPathReader = new XPathReader(xmlTextReader, xPathCollection);

            while (xPathReader.ReadUntilMatch())
            {
                if (xPathReader.Match(_XPathQuery))
                {
                    rootNodeName = xPathReader.LocalName;
                    rootNodeNamespace = xPathReader.NamespaceURI;
                }
            }

            inmsg.Context.Promote(BizTalkGlobalPropertySchemaEnum.MessageType.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace, string.Format("{0}#{1}", rootNodeNamespace, rootNodeName));
            inmsg.BodyPart.Data.Position = 0;
        }
    }
}