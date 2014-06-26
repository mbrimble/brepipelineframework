using System;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class RemoveContextPropertyPipelineInstruction : IBREPipelineInstruction
    {
        private string propertyName;
        private string propertyNamespace;


        public RemoveContextPropertyPipelineInstruction(string propertyName, string propertyNamespace)
        {
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            inmsg.Context.Write(propertyName, propertyNamespace, null);
        }
    }
}
