using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;
using Microsoft.BizTalk.XPath;
using System.Text.RegularExpressions;
using Microsoft.BizTalk.Component.Interop;
using System;
using System.Collections.Generic;

namespace BREPipelineFramework
{
    public class MessageUtility
    {
        #region Private properties

        private Stream documentStream;
        private string bodyString;
        private string rootNodeName;
        private string rootNodeNamespace;
        private bool messageTypePropertiesSet;
        private IPipelineContext pc;

        #endregion

        #region Constructors

        public MessageUtility(Stream DocumentStream, IPipelineContext pc)
        {
            this.documentStream = DocumentStream;
            this.messageTypePropertiesSet = false;
            this.pc = pc;
        }

        public MessageUtility(Stream DocumentStream, string messageType, IPipelineContext pc)
        {
            this.documentStream = DocumentStream;
            char[] delimiters = {'#'};
            string[] messageTypeArray = messageType.Split(delimiters, 2);

            if (messageTypeArray.Length == 2)
            {
                this.rootNodeNamespace = messageTypeArray[0];
                this.rootNodeName = messageTypeArray[1];
            }
            else
            {
                this.rootNodeName = messageTypeArray[0];
                this.rootNodeNamespace = string.Empty;
            }

            this.messageTypePropertiesSet = true;
            this.pc = pc;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Check if a string exists in the message body
        /// </summary>
        public bool CheckIfStringExistsInMessage(string stringToFind)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(documentStream);
            pc.ResourceTracker.AddResource(reader);
            bool found = false;

            while (!reader.EndOfStream && !found)
            {
                string body = reader.ReadLine();
                found = body.Contains(stringToFind);
            }

            documentStream.Position = 0;
            return found;
        }

        /// <summary>
        /// Check if a regex finds a match in the message body
        /// </summary>
        public bool CheckIfRegexEvaluatesInMessage(string regexToFind)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(documentStream);
            pc.ResourceTracker.AddResource(reader);
            bool found = false;

            while (!reader.EndOfStream && !found)
            {
                string body = reader.ReadLine();
                Match match = Regex.Match(body, regexToFind);
                found = match.Success;
            }

            documentStream.Position = 0;
            return found;
        }

        /// <summary>
        /// Returns the first regex match from the message body
        /// </summary>
        public string ReturnFirstRegexMatch(string regexToFind)
        {
            return ReturnRegexMatchByIndex(regexToFind, 0);
        }

        /// <summary>
        /// Returns a regex match from the message body by index
        /// </summary>
        public string ReturnRegexMatchByIndex(string regexToFind, int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(documentStream);
            pc.ResourceTracker.AddResource(reader);
            string matchedString = String.Empty;
            List<string> matchedStrings = new List<string>();

            while (!reader.EndOfStream && matchedStrings.Count < index + 1)
            {
                string body = reader.ReadLine();
                MatchCollection matchCollection = Regex.Matches(body, regexToFind);

                foreach (Match match in matchCollection)
                {
                    matchedStrings.Add(match.Value);
                }
            }

            if (matchedStrings.Count >= index + 1)
            {
                matchedString = matchedStrings[index].ToString();
            }

            documentStream.Position = 0;
            return matchedString;           
        }

        /// <summary>
        /// Get the length of the message body
        /// </summary>
        public int MessageBodyLength()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(documentStream);
            pc.ResourceTracker.AddResource(reader);
            int length = 0;

            while (!reader.EndOfStream)
            {
                string body = reader.ReadLine();
                length = length + body.Length;
            }

            documentStream.Position = 0;
            return length;
        }

        /// <summary>
        /// Get the root node name of the XML message contained within the body stream
        /// </summary>
        /// <returns></returns>
        public string GetMessageRootNodeName()
        {
            if (!messageTypePropertiesSet)
            {
                SetMessageTypeVariables();
            }

            return rootNodeName;           
        }

        /// <summary>
        /// Get the root node namespace of the XML message contained within the body stream
        /// </summary>
        /// <returns></returns>
        public string GetMessageRootNodeNamespace()
        {
            if (!messageTypePropertiesSet)
            {
                SetMessageTypeVariables();
            }

            return rootNodeNamespace;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get a node name or namespace based on an XPATH query run against the body stream
        /// </summary>
        /// <param name="_XPathResultType"></param>
        /// <param name="_XPathQuery"></param>
        /// <returns></returns>
        private void SetMessageTypeVariables()
        {
            string _XPathQuery = "/*";
            
            try
            {
                XmlTextReader xmlTextReader = new XmlTextReader(documentStream);
                XPathCollection xPathCollection = new XPathCollection();
                xPathCollection.Add(_XPathQuery);

                XPathReader xPathReader = new XPathReader(xmlTextReader, xPathCollection);

                while (xPathReader.ReadUntilMatch())
                {
                    if (xPathReader.Match(_XPathQuery))
                    {
                        rootNodeName = xPathReader.LocalName;
                        rootNodeNamespace = xPathReader.NamespaceURI;
                        messageTypePropertiesSet = true;
                    }
                }
            }
            catch
            {
                //Don't rethrow the exception, if it's a flat file set message type properties to blank values
                rootNodeName = "";
                rootNodeNamespace = "";
                messageTypePropertiesSet = true;
            }

            documentStream.Position = 0;
        }

        #endregion
    }
}
