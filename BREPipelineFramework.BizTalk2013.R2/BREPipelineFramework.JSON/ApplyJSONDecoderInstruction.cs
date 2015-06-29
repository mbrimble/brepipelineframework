using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.JSON
{
    public class ApplyJSONDecoderInstruction : IBREPipelineInstruction
    {
        private JsonDecoder decoder = new JsonDecoder();

        public ApplyJSONDecoderInstruction(string rootNodeName, string rootNodeNamespace)
        {
            decoder.RootNode = rootNodeName;
            decoder.RootNodeNamespace = rootNodeNamespace;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            inmsg = PipelineExecutionHelper.Execute(decoder, inmsg, pc);
        }
    }
}
