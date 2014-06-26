using Microsoft.RuleEngine;
using System.IO;
using System.Xml;
using Microsoft.BizTalk.Streaming;
using BREPipelineFramework.Helpers.Tracing;

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
        private Microsoft.BizTalk.Component.Interop.IPipelineContext pc;

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
        public TypedXMLDocumentWrapper(Stream DocumentStream, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            this.documentStream = DocumentStream;
            this.pc = pc;
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
            XmlTextReader reader = new XmlTextReader(documentStream);
            document = new TypedXmlDocument(DocumentType, reader);
            documentStream.Position = 0;
            pc.ResourceTracker.AddResource(reader);

            hasBeenSet = true;
        }

        /// <summary>
        /// Static method to apply a TypedXMLDocument to a BizTalk message body
        /// </summary>
        /// <param name="document"></param>
        /// <param name="inmsg"></param>
        /// <param name="pc"></param>
        /// <param name="callToken"></param>
        public static void ApplyTypedXMLDocument(TypedXmlDocument document, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc, string callToken)
        {
            TraceManager.PipelineComponent.TraceInfo("{0} - Applying typed XML document (overwriting current message body)", callToken);

            XmlDocument doc = (XmlDocument)document.Document;
            VirtualStream ms = new VirtualStream();
            doc.Save(ms);
            ms.Position = 0;
            inmsg.BodyPart.Data = ms;

            // Add the new message body part's stream to the Pipeline Context's resource tracker so that it will be disposed off correctly
            pc.ResourceTracker.AddResource(ms);
        }

        #endregion
    }
}
