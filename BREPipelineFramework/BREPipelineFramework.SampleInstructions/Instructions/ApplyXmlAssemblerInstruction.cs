using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyXmlAssemblerInstruction : IBREPipelineInstruction
    {
        private XmlAsmComp assembler = new XmlAsmComp();

        public ApplyXmlAssemblerInstruction(string envelopeSpecName)
        {
            assembler.EnvelopeDocSpecNames = new Microsoft.BizTalk.Component.Utilities.SchemaList();
            assembler.EnvelopeDocSpecNames.Add(new Microsoft.BizTalk.Component.Utilities.Schema(envelopeSpecName));
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Assemble(assembler, inmsg, pc);
        }
    }
}
