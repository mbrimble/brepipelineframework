using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.RuleEngine;
using System.IO;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyTypedXMLDocumentInstruction : IBREPipelineInstruction
    {
        private TypedXmlDocument document;
        private XMLFactsApplicationStageEnum xmlFactsApplicationStage;

        public ApplyTypedXMLDocumentInstruction(TypedXmlDocument document, XMLFactsApplicationStageEnum xmlFactsApplicationStage)
        {
            this.document = document;
            this.xmlFactsApplicationStage = xmlFactsApplicationStage;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            if (xmlFactsApplicationStage == XMLFactsApplicationStageEnum.Explicit)
            {
                TypedXMLDocumentWrapper.ApplyTypedXMLDocument(document, inmsg, pc);
            }
        }
    }
}
