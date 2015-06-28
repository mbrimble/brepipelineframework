using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipleineFramework.JSON
{
    public class ApplyJSONEncoderInstruction : IBREPipelineInstruction
    {
        private JsonEncoder encoder = new JsonEncoder();

        public ApplyJSONEncoderInstruction(bool removeOuterEnvelope)
        {
            encoder.RemoveOuterEnvelope = removeOuterEnvelope;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Execute(encoder, inmsg, pc);
        }
    }
}