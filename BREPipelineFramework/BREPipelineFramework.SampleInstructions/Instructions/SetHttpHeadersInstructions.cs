using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.Helpers.Tracing;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class SetHttpHeadersInstructions : IBREPipelineInstruction
    {
        private Dictionary<string, string> outboundHTTPHeadersCollection = new Dictionary<string, string>();
        string callToken;

        public SetHttpHeadersInstructions(Dictionary<string, string> outboundHTTPHeadersCollection, string callToken)
        {
            this.outboundHTTPHeadersCollection = outboundHTTPHeadersCollection;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            StringBuilder outboundHeadersBuilder = new StringBuilder();

            foreach (KeyValuePair<string, string> kp in outboundHTTPHeadersCollection)
            {
                outboundHeadersBuilder.AppendFormat("{0}: {1}", kp.Key, kp.Value);
                outboundHeadersBuilder.Append(Environment.NewLine);
            }

            string outboundHeaders = outboundHeadersBuilder.ToString();

            TraceManager.PipelineComponent.TraceInfo(callToken + " - Adding outbound HTTP headers to the message with the below value " + Environment.NewLine + outboundHeaders);
            inmsg.Context.Write(BizTalkWCFPropertySchemaEnum.HttpHeaders.ToString(), ContextPropertyNamespaces._WCFPropertyNamespace, outboundHeaders);
            inmsg.Context.Write(BizTalkGlobalPropertySchemaEnum.IsDynamicSend.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace, true);
        }
    }
}
