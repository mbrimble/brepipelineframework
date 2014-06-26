using Microsoft.RuleEngine;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyTypedXMLDocumentInstruction : IBREPipelineInstruction
    {
        private TypedXmlDocument document;
        private XMLFactsApplicationStageEnum xmlFactsApplicationStage;
        private string callToken;

        public ApplyTypedXMLDocumentInstruction(TypedXmlDocument document, XMLFactsApplicationStageEnum xmlFactsApplicationStage, string callToken)
        {
            this.document = document;
            this.xmlFactsApplicationStage = xmlFactsApplicationStage;
            this.callToken = callToken;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            if (xmlFactsApplicationStage == XMLFactsApplicationStageEnum.Explicit)
            {
                TypedXMLDocumentWrapper.ApplyTypedXMLDocument(document, inmsg, pc, callToken);
            }
        }
    }
}
