using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
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
            try
            {
                inmsg.Context.Write(propertyName, propertyNamespace, null);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to remove context property " + propertyNamespace + "#" + propertyName + ". Encountered error - " + e.ToString());
            }
        }
    }
}
