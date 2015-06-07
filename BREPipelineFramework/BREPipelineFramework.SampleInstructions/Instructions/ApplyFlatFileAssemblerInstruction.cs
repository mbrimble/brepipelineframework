using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyFlatFileAssemblerInstruction : IBREPipelineInstruction
    {
        private FFAsmComp assembler = new FFAsmComp();

        public ApplyFlatFileAssemblerInstruction(string headerSpecName = null, string trailerSpecName = null,
            bool validateDocumentStructure = false)
        {
            if (!string.IsNullOrEmpty(headerSpecName))
            {
                assembler.HeaderSpecName = new Microsoft.BizTalk.Component.Utilities.SchemaWithNone(headerSpecName);
            }

            if (!string.IsNullOrEmpty(trailerSpecName))
            {
                assembler.TrailerSpecName = new Microsoft.BizTalk.Component.Utilities.SchemaWithNone(trailerSpecName);
            }
        }
        
        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Assemble(assembler, inmsg, pc);
        }
    }
}
