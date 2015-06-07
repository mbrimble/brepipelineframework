using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Component;
using BREPipelineFramework.Helpers;
using System.IO;
using Microsoft.BizTalk.Streaming;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class ApplyFlatFileDisassemblerInstruction : IBREPipelineInstruction
    {
        private FFDasmComp disassembler = new FFDasmComp();

        public ApplyFlatFileDisassemblerInstruction(string documentSpecName, string headerSpecName = null, bool preserveHeader = false, string trailerSpecName = null,
            bool validateDocumentStructure = false)
        {
            disassembler.DocumentSpecName = new Microsoft.BizTalk.Component.Utilities.SchemaWithNone(documentSpecName);

            if (!string.IsNullOrEmpty(headerSpecName))
            {
                disassembler.HeaderSpecName = new Microsoft.BizTalk.Component.Utilities.SchemaWithNone(headerSpecName);
                disassembler.PreserveHeader = preserveHeader;
            }

            if (!string.IsNullOrEmpty(trailerSpecName))
            {
                disassembler.TrailerSpecName = new Microsoft.BizTalk.Component.Utilities.SchemaWithNone(trailerSpecName);
            }

            disassembler.ValidateDocumentStructure = validateDocumentStructure;
        }
        
        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Disassemble(disassembler, inmsg, pc);
        }
    }
}
