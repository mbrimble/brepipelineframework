using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework.SampleInstructions.Instructions;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class PipelineMetaInstructions : BREPipelineMetaInstructionBase
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

        public void ValidateMessage()
        {
            ApplyXmlValidatorInstruction instruction = new ApplyXmlValidatorInstruction();
            base.AddInstruction(instruction);
        }

        public void AssembleXMLMessageWithEnvelope(string envelopeSpecName)
        {
            ApplyXmlAssemblerInstruction instruction = new ApplyXmlAssemblerInstruction(envelopeSpecName);
            base.AddInstruction(instruction);
        }

        public void DisassembleXMLMessage()
        {
            ApplyXmlDisassemblerInstruction instruction = new ApplyXmlDisassemblerInstruction();
            base.AddInstruction(instruction);
        }

        public void DisassembleXMLMessagePropertyPromotionOnly()
        {
            SetContextPropertyPipelineInstruction instruction = new SetContextPropertyPipelineInstruction(BizTalkXMLNORMPropertySchemaEnum.PromotePropertiesOnly.ToString(),
                ContextPropertyNamespaces._XMLNormPropertyNamespace, true, ContextInstructionTypeEnum.Write);
            base.AddInstruction(instruction);

            ApplyXmlDisassemblerInstruction disassemblyInstruction = new ApplyXmlDisassemblerInstruction();
            base.AddInstruction(disassemblyInstruction);
        }
    }
}
