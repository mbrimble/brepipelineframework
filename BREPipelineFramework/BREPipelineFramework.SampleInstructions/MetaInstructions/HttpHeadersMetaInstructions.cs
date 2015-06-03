using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework.Helpers.Tracing;
using BREPipelineFramework.Helpers;
using System.Text.RegularExpressions;
using BREPipelineFramework.SampleInstructions.Instructions;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class HttpHeadersMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private fields

        private Dictionary<string, string> inboundHTTPHeadersCollection = new Dictionary<string, string>();
        private Dictionary<string, string> outboundHTTPHeadersCollection = new Dictionary<string, string>();
        private bool inboundHTTPHeadersExtracted = false;
        private SetHttpHeadersInstructions instruction = null;

        #endregion

        #region Public methods

        // Get an inbound HTTP header value
        public string GetHTTPHeaderValue(string headerName, FailureActionEnum failureAction)
        {
            ScrapeInboundHTTPHeaders();
            string headerValue = null;

            if (inboundHTTPHeadersCollection.ContainsKey(headerName))
            {
                headerValue = inboundHTTPHeadersCollection[headerName];
            }
            else
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Could not find inbound HTTP Header " + headerName);
                    base.SetException(exc);
                }
                else if (failureAction == FailureActionEnum.BlankOrDefaultValue)
                {
                    headerValue = string.Empty;
                }
                else if (failureAction == FailureActionEnum.Null)
                {
                    // Do nothing, leave as null
                }
            }
            return headerValue;
        }

        // Copy an inbound HTTP header to the outbound HTTP headers
        public void CopyInboundToOutboundHTTPHeader(string headerName, CacheFailureEnum failure)
        {
            ScrapeInboundHTTPHeaders();

            if (inboundHTTPHeadersCollection.ContainsKey(headerName))
            {
                string headerValue = inboundHTTPHeadersCollection[headerName];
                AddOrUpdateOutboundHTTPHeader(headerName, headerValue);
            }
            else
            {
                if (failure == CacheFailureEnum.RaiseAnException)
                {
                    Exception exc = new Exception("Could not find inbound HTTP Header " + headerName);
                    base.SetException(exc);
                }
                else if (failure == CacheFailureEnum.IgnoreAndCarryOn)
                {
                    // Take no action
                }
            }
        }

        // Add or update outbound HTTP headers
        public void AddOrUpdateOutboundHTTPHeader(string headerName, string headerValue)
        {          
            if (outboundHTTPHeadersCollection.ContainsKey(headerName))
            {
                outboundHTTPHeadersCollection[headerName] = headerValue;
            }
            else
            {
                outboundHTTPHeadersCollection.Add(headerName, headerValue);
            }

            if (instruction == null)
            {
                instruction = new SetHttpHeadersInstructions(outboundHTTPHeadersCollection, CallToken);
                base.AddInstruction(instruction);
            }
        }

        #endregion

        #region Private methods

        //Parse the inbound HTTP headers from the WCF.InboundHttpHeaders context property
        private void ScrapeInboundHTTPHeaders()
        {
            if (inboundHTTPHeadersExtracted == false)
            {
                object propertyValueObj = base.InMsg.Context.Read(BizTalkWCFPropertySchemaEnum.InboundHttpHeaders.ToString(), ContextPropertyNamespaces._WCFPropertyNamespace);

                if (propertyValueObj != null)
                {
                    string propertyValue = propertyValueObj.ToString();

                    string pattern = @"\r\n";
                    List<string> inboundHeaders = new List<string>(Regex.Split(propertyValue, pattern));

                    foreach (string header in inboundHeaders)
                    {
                        List<string> headers = new List<string>(Regex.Split(header, ": "));

                        if (headers.Count == 2)
                        {
                            inboundHTTPHeadersCollection.Add(headers[0], headers[1]);
                        }
                    }
                }

                inboundHTTPHeadersExtracted = true;
            }
        }

        #endregion
    }
}
