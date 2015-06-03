using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.Helpers
{
    public static class PipelineExecutionHelper
    {
        public static Microsoft.BizTalk.Message.Interop.IBaseMessage Disassemble(IDisassemblerComponent disassembler, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            disassembler.Disassemble(pc, inmsg);

            inmsg = disassembler.GetNext(pc);
            inmsg.BodyPart.Data.Position = 0;

            return inmsg;
        }

        public static Microsoft.BizTalk.Message.Interop.IBaseMessage Assemble(IAssemblerComponent assembler, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            assembler.AddDocument(pc, inmsg);
            inmsg = assembler.Assemble(pc);
            inmsg.BodyPart.Data.Position = 0;
            return inmsg;
        }
    }
}
