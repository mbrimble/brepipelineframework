using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework.SampleInstructions.Instructions;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class FlatFileMetaInstructions : BREPipelineMetaInstructionBase
    {
        public void DisassembleFlatFile(string documentSpecName)
        {
            ApplyFlatFileDisassemblerInstruction instruction = new ApplyFlatFileDisassemblerInstruction(documentSpecName);
            base.AddInstruction(instruction);
        }

        public void DisassembleFlatFileWithHeader(string documentSpecName, string headerSpecName, bool preserveHeader)
        {
            ApplyFlatFileDisassemblerInstruction instruction = new ApplyFlatFileDisassemblerInstruction(documentSpecName, headerSpecName, preserveHeader);
            base.AddInstruction(instruction);
        }

        public void DisassembleFlatFileWithHeaderAndTrailer(string documentSpecName, string headerSpecName, bool preserveHeader, string trailerSpecName)
        {
            ApplyFlatFileDisassemblerInstruction instruction = new ApplyFlatFileDisassemblerInstruction(documentSpecName, headerSpecName, preserveHeader, trailerSpecName);
            base.AddInstruction(instruction);
        }

        public void DisassembleFlatFileWithTrailer(string documentSpecName, string trailerSpecName)
        {
            ApplyFlatFileDisassemblerInstruction instruction = new ApplyFlatFileDisassemblerInstruction(documentSpecName, trailerSpecName: trailerSpecName);
            base.AddInstruction(instruction);
        }

        public void AssembleFlatFile()
        {
            ApplyFlatFileAssemblerInstruction instruction = new ApplyFlatFileAssemblerInstruction();
            base.AddInstruction(instruction);
        }

        public void AssembleFlatFileWithHeader(string headerSpecName)
        {
            ApplyFlatFileAssemblerInstruction instruction = new ApplyFlatFileAssemblerInstruction(headerSpecName);
            base.AddInstruction(instruction);
        }

        public void AssembleFlatFileWithHeaderAndTrailer(string headerSpecName, string trailerSpecName)
        {
            ApplyFlatFileAssemblerInstruction instruction = new ApplyFlatFileAssemblerInstruction(headerSpecName, trailerSpecName);
            base.AddInstruction(instruction);
        }

        public void AssembleFlatFileWithTrailer(string trailerSpecName)
        {
            ApplyFlatFileAssemblerInstruction instruction = new ApplyFlatFileAssemblerInstruction(trailerSpecName: trailerSpecName);
            base.AddInstruction(instruction);
        }
    }
}
