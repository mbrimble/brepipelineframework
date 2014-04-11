using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;
using Microsoft.BizTalk.XPath;
using System.Text.RegularExpressions;

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

        #endregion

        #region Constructors

        public MessageUtility(Stream DocumentStream)
        {
            this.documentStream = DocumentStream;
            this.messageTypePropertiesSet = false;
        }

        public MessageUtility(Stream DocumentStream, string messageType)
        {
            this.documentStream = DocumentStream;
            char[] delimiters = {'#'};
            string[] messageTypeArray = messageType.Split(delimiters, 2);
            this.rootNodeNamespace = messageTypeArray[0];
            this.rootNodeName = messageTypeArray[1];
            this.messageTypePropertiesSet = true;
        }

        #endregion

        #region Public methods

        public bool CheckIfStringExistsInMessage(string stringToFind)
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            return bodyString.Contains(stringToFind);
        }

        public bool CheckIfRegexEvaluatesInMessage(string regexToFind)
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            return Regex.IsMatch(bodyString, regexToFind);
        }

        public string ReturnFirstRegexMatch(string regexToFind)
        {
            return ReturnRegexMatchByIndex(regexToFind, 0);
        }

        public string ReturnRegexMatchByIndex(string regexToFind, int index)
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            Match match = Regex.Match(bodyString, regexToFind);

            if (match.Success)
            {
                try
                {
                    return match.Groups[index].Value;
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Get the length of the message body
        /// </summary>
        public int MessageBodyLength()
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            try
            {
                return bodyString.Length;
            }
            catch
            {
                return 0;
            }
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
            VirtualStream virtualStream = new VirtualStream();
            ReadOnlySeekableStream readOnlySeekableStream = new ReadOnlySeekableStream(documentStream, virtualStream);

            try
            {
                XmlTextReader xmlTextReader = new XmlTextReader(readOnlySeekableStream);
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

            documentStream.Seek(0, SeekOrigin.Begin);
        }

        private void ExtractBody()
        {
            VirtualStream newMsgStream = new VirtualStream();
            documentStream.CopyTo(newMsgStream);
            documentStream.Seek(0, SeekOrigin.Begin);
            newMsgStream.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader reader = new System.IO.StreamReader(newMsgStream);
            bodyString = reader.ReadToEnd();
            reader.Close();
        }

        #endregion
    }
}
