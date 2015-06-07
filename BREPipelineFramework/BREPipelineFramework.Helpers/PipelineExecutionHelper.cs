using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;

namespace BREPipelineFramework.Helpers
{
    public static class PipelineExecutionHelper
    {
        public static Microsoft.BizTalk.Message.Interop.IBaseMessage Disassemble(IDisassemblerComponent disassembler, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            disassembler.Disassemble(pc, inmsg);
            IBaseMessage message = disassembler.GetNext(pc);
            pc.ResourceTracker.AddResource(message);

            return message;
        }

        public static Microsoft.BizTalk.Message.Interop.IBaseMessage Assemble(IAssemblerComponent assembler, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            assembler.AddDocument(pc, inmsg);
            IBaseMessage message = assembler.Assemble(pc);
            pc.ResourceTracker.AddResource(message);
            
            return message;
        }

        public static Microsoft.BizTalk.Message.Interop.IBaseMessage Execute(IComponent component, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            IBaseMessage message = component.Execute(pc, inmsg);
            pc.ResourceTracker.AddResource(message);

            return message;
        }
    }
}
