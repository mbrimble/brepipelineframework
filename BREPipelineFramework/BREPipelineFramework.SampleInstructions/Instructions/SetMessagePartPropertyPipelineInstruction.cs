using System;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class SetMessagePartDetailsPipelineInstruction : IBREPipelineInstruction
    {
        private string value;
        private PartDetailTypeEnum detailType;
        private string partName;

        public SetMessagePartDetailsPipelineInstruction(string value, PartDetailTypeEnum detailType, string partName)
        {
            this.value = value;
            this.detailType = detailType;
            this.partName = partName;
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            if (detailType == PartDetailTypeEnum.CharSet)
            {
                inmsg.GetPart(partName).Charset = value;
            }
            else if (detailType == PartDetailTypeEnum.ContentType)
            {
                inmsg.GetPart(partName).ContentType = value;
            }

        }
    }
}
