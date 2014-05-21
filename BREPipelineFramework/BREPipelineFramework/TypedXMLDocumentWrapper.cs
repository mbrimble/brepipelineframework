using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.RuleEngine;
using System.IO;
using System.Xml;
using Microsoft.BizTalk.Streaming;
using Microsoft.BizTalk.XPath;

namespace BREPipelineFramework
{
    /// <summary>
    /// Used to wrap a TypedXmlDocument instance with logic exposed in BRE, also contains some utility methods to evaluate the current message
    /// </summary>
    public class TypedXMLDocumentWrapper
    {
        #region Private properties

        private TypedXmlDocument document;
        private Stream documentStream;
        private bool hasBeenSet;

        #endregion

        #region Public properties

        /// <summary>
        /// The TypedXmlDocument to be asserted to the ExecutionPolicy
        /// </summary>
        public TypedXmlDocument Document
        {
            get { return document; }
        }

        /// <summary>
        /// Indicates whether the TypedXMLDocumentWrapper has been setup in an InstructionLoaderPolicy or not
        /// </summary>
        public int DocumentCount
        {
            get 
            {
                if (hasBeenSet)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate the TypedXMLDocumentWrapper passing in the original message body stream
        /// </summary>
        /// <param name="DocumentStream"></param>
        public TypedXMLDocumentWrapper(Stream DocumentStream)
        {
            this.documentStream = DocumentStream;
            hasBeenSet = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Use a DocumentType and the documentStream setup in the constructor to create a TypedXmlDocument property
        /// </summary>
        /// <param name="DocumentType"></param>
        public void CreateTypedXmlDocument(string DocumentType)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.Load(documentStream);
            document = new TypedXmlDocument(DocumentType, doc);
            documentStream.Seek(0, SeekOrigin.Begin);
            hasBeenSet = true;
        }

        public static void ApplyTypedXMLDocument(TypedXmlDocument document, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            XmlDocument doc = (XmlDocument)document.Document;
            byte[] output = System.Text.Encoding.UTF8.GetBytes(doc.ToString());
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            inmsg.BodyPart.Data = ms;

            // Add the new message body part's stream to the Pipeline Context's resource tracker so that it will be disposed off correctly
            pc.ResourceTracker.AddResource(ms);
        }

        #endregion
    }
}
