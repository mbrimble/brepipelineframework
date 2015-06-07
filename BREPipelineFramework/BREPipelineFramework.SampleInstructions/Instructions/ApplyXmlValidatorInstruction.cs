using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyXmlValidatorInstruction : IBREPipelineInstruction
    {
        private XmlValidator component = new XmlValidator();

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Execute(component, inmsg, pc);
        }
    }
}
