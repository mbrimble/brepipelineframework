using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyXmlDisassemblerInstruction : IBREPipelineInstruction
    {
        private XmlDasmComp disassembler = new XmlDasmComp();

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Disassemble(disassembler, inmsg, pc);
        }
    }
}
