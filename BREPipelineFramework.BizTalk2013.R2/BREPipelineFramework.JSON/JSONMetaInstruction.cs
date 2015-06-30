using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.Helpers.Tracing;
using BREPipelineFramework.SampleInstructions;
using BREPipelineFramework.SampleInstructions.MetaInstructions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace BREPipelineFramework.JSON
{
    public class JSONMetaInstruction : BREPipelineMetaInstructionBase
    {
        private static MemoryCache cache = MemoryCache.Default;

        #region Public Methods

        public void DecodeJSONWithNamespaceSpecified(string rootNodeName, string rootNodeNamespace)
        {
            ApplyJSONDecoderInstruction instruction = new ApplyJSONDecoderInstruction(rootNodeName, rootNodeNamespace);
            base.AddInstruction(instruction);
        }

        public void EncodeJSON(JSONRemoveEnvelopeEnum removeOuterEnvelopeEnum)
        {
            bool removeOuterEnvelope = false;

            if (removeOuterEnvelopeEnum == JSONRemoveEnvelopeEnum.RemoveOuterEnvelope)
            {
                removeOuterEnvelope = true;
            }
            else if (removeOuterEnvelopeEnum == JSONRemoveEnvelopeEnum.DoNotRemoveOuterEnvelope)
            {
                removeOuterEnvelope = false;
            }

            ApplyJSONEncoderInstruction instruction = new ApplyJSONEncoderInstruction(removeOuterEnvelope);
            base.AddInstruction(instruction);
        }

        public void CacheAcceptHeader()
        {
            string acceptHeader = ExtractInboundHttpHeader("Accept");

            if (!string.IsNullOrEmpty(acceptHeader))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddMilliseconds(TimeHelper.GetTimeInMilliseconds(10, TimeEnum.Minutes));
                policy.Priority = CacheItemPriority.NotRemovable;
                string key = GetCachingKey();
                cache.Set(key, acceptHeader, policy, null);
            }
        }

        public string GetCachedAcceptHeader()
        {
            string key = GetCachingKey();
            object acceptHeaderObj = cache[key];
            string acceptHeader = string.Empty;

            if (acceptHeaderObj != null)
            {
                acceptHeader = acceptHeaderObj.ToString();
            }

            return acceptHeader;
        }

        public bool AssessCachedAcceptHeader(ContentTypeEnum assessedContentType)
        {
            ContentTypeEnum actualContentType = ContentTypeEnum.NotResolved;

            string key = GetCachingKey();
            object acceptHeaderObj = cache[key];
            string acceptHeader = string.Empty;

            if (acceptHeaderObj != null)
            {
                acceptHeader = acceptHeaderObj.ToString();
            }

            if (!string.IsNullOrEmpty(acceptHeader))
            {
                ChooseContentType(ref actualContentType, acceptHeader);
                TraceManager.PipelineComponent.TraceInfo("{0} - Used cached Accept header to determine that the response content type is {1}", CallToken, actualContentType.ToString());
            }

            return (assessedContentType == actualContentType);
        }

        public bool AssessContentType(ContentTypeEnum assessedContentType)
        {
            bool contentTypeSet = false;
            ContentTypeEnum actualContentType = ContentTypeEnum.NotResolved;

            string contentType = ExtractInboundHttpHeader("Content-Type");

            if (!string.IsNullOrEmpty(contentType))
            {
                ChooseContentType(ref actualContentType, contentType);
                contentTypeSet = true;
                TraceManager.PipelineComponent.TraceInfo("{0} - Used Content-Type HTTP header to determine that the request content type is {1}", CallToken, actualContentType.ToString());
            }

            if (!contentTypeSet)
            {
                ReflectContentTypeBasedOnMessageContent(ref contentTypeSet, ref actualContentType);
            }

            if (!contentTypeSet)
            {
                TraceManager.PipelineComponent.TraceWarning("{0} - Could not determine the request content type, as Content-Type header was either blank or not provided, and request body appeared to be neither XML nor JSON", CallToken);
            }

            return (assessedContentType == actualContentType);
        }

        public void ExecuteXSLTTransform(string _XSLTPath)
        {
            // Get the full path to the Xslt file
            if (!System.IO.File.Exists(_XSLTPath))
            {
                throw new ArgumentException("The XSL transformation file " + _XSLTPath + " can not be found");
            }

            // Load transform
            XslTransform transform = new XslTransform();
            transform.Load(_XSLTPath);

            //Load Xml stream in XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(InMsg.BodyPart.GetOriginalDataStream());

            //Create memory stream to hold transformed data.
            MemoryStream ms = new MemoryStream();

            //Preform transform
            transform.Transform(doc, null, ms, null);
            ms.Seek(0, SeekOrigin.Begin);

            Pc.ResourceTracker.AddResource(ms);

            InMsg.BodyPart.Data = ms;
            InMsg.BodyPart.ContentType = "text/html";
        }

        #endregion

        #region Private Methods

        private void ReflectContentTypeBasedOnMessageContent(ref bool contentTypeSet, ref ContentTypeEnum actualContentType)
        {
            TraceManager.PipelineComponent.TraceWarning("{0} - No Content-Type HTTP header was found so will have to resort to reading a portion of the stream to determine the request content type", CallToken);

            string firstChar = string.Empty;
            StreamReader reader = new StreamReader(InMsg.BodyPart.GetOriginalDataStream());
            Pc.ResourceTracker.AddResource(reader);

            string firstLine = string.Empty;

            while (!reader.EndOfStream && (firstLine == string.Empty))
            {
                firstLine = reader.ReadLine().Trim();
                TraceManager.PipelineComponent.TraceInfo("{0} - Reading line from stream to try to determine the request content type", CallToken, firstChar, actualContentType.ToString());
            }

            InMsg.BodyPart.Data.Position = 0;

            if (firstLine.Length > 0)
            {
                firstChar = firstLine.Substring(0, 1);

                switch (firstChar)
                {
                    case "<":
                        actualContentType = ContentTypeEnum.Xml;
                        contentTypeSet = true;
                        break;
                    case "{":
                        actualContentType = ContentTypeEnum.Json;
                        contentTypeSet = true;
                        break;
                    case "[":
                        actualContentType = ContentTypeEnum.Json;
                        contentTypeSet = true;
                        break;
                    default:
                        actualContentType = ContentTypeEnum.NotResolved;
                        break;
                }
            }

            if (contentTypeSet)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Used first character in request body {1} to determine that the request content type is {2}", CallToken, firstChar, actualContentType.ToString());
            }
        }

        private static void ChooseContentType(ref ContentTypeEnum actualContentType, string contentType)
        {
            if (contentType.ToLower().Contains("application/json") || contentType.ToLower().Contains("text/json") ||
                contentType.ToLower().Contains("text/javascript"))
            {
                actualContentType = ContentTypeEnum.Json;
            }
            else if (contentType.ToLower().Contains("application/xml") || contentType.ToLower().Contains("text/xml"))
            {
                actualContentType = ContentTypeEnum.Xml;
            }
            else if (contentType.ToLower().Contains("application/x-www-form-urlencoded"))
            {
                actualContentType = ContentTypeEnum.URLEncodedForm;
            }
            else if (contentType.ToLower().Contains("application/*"))
            {
                actualContentType = ContentTypeEnum.Json;
            }
            else if (contentType.ToLower().Contains("text/csv"))
            {
                actualContentType = ContentTypeEnum.CSV;
            }
            else if (contentType.ToLower().Contains("text/html"))
            {
                actualContentType = ContentTypeEnum.HTML;
            }
            else if (contentType.ToLower().Contains("text/plain"))
            {
                actualContentType = ContentTypeEnum.Text;
            }
            else if (contentType.Trim() != string.Empty)
            {
                actualContentType = ContentTypeEnum.Other;
            }
        }

        private string GetCachingKey()
        {
            string interchangeId = InMsg.Context.Read(BizTalkGlobalPropertySchemaEnum.InterchangeID.ToString(),
                ContextPropertyNamespaces._BTSPropertyNamespace).ToString();
            string key = String.Format("BRE Pipeline Framework Accept Header Caching {0}", interchangeId);
            return key;
        }

        private string ExtractInboundHttpHeader(string headerName)
        {
            object inboundHttpHeadersObj = base.InMsg.Context.Read(BizTalkWCFPropertySchemaEnum.InboundHttpHeaders.ToString(), ContextPropertyNamespaces._WCFPropertyNamespace);
            string _HTTPHeader = string.Empty;

            if (inboundHttpHeadersObj != null)
            {
                string inboundHttpHeaders = inboundHttpHeadersObj.ToString();

                string pattern = @"\r\n";
                List<string> inboundHeaders = new List<string>(Regex.Split(inboundHttpHeaders, pattern));

                foreach (string header in inboundHeaders)
                {
                    List<string> headers = new List<string>(Regex.Split(header, ": "));

                    if (headers.Count == 2)
                    {
                        if (headers[0] == headerName)
                        {
                            _HTTPHeader = headers[1];
                        }
                    }
                }
            }

            return _HTTPHeader;
        }

        #endregion
    }
}
