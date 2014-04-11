using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.XPath;

namespace BREPipelineFramework.Helpers
{
    /// <summary>
    /// Class that represents an Instruction to execute an XPath Query against a message and write the result to the context of a message
    /// </summary>
    public class XPathInstruction
    {
        #region Private Properties

        private string _XPathQuery;
        private ContextInstructionTypeEnum promotion;
        private XPathResultTypeEnum _XPathResultType;
        private string propertyName;
        private string propertyNamespace;
        private TypeEnum type;
        private bool isFound = false;
        private bool exceptionIfNotFound = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// The XPath Query to be executed
        /// </summary>
        public string XPathQuery
        {
            get { return _XPathQuery; }
            set { _XPathQuery = value; }
        }

        /// <summary>
        /// Whether the result of the XPath Query should be promoted or written to the context
        /// </summary>
        public ContextInstructionTypeEnum Promotion
        {
            get { return promotion; }
            set { promotion = value; }
        }

        /// <summary>
        /// Whether the XPath Query's resulting node's value, name, or namespace should be treated as the result
        /// </summary>
        public XPathResultTypeEnum XPathResultType
        {
            get { return _XPathResultType; }
            set { _XPathResultType = value; }
        }

        /// <summary>
        /// The name of the context property that the XPath Query's result will be written to
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// The namespace of the context property that the XPath Query's result will be written to
        /// </summary>
        public string PropertyNamespace
        {
            get { return propertyNamespace; }
            set { propertyNamespace = value; }
        }

        /// <summary>
        /// The type that the XPath Query's result will be cast to
        /// </summary>
        public TypeEnum Type
        {
            get { return type; }
            set { type = value; }
        }
        
        /// <summary>
        /// Keeps track of whether or not the XPath Query has succesfully evaluated
        /// </summary>
        public bool IsFound
        {
            get { return isFound; }
            set { isFound = value; }
        }
        
        /// <summary>
        /// Specifies whether an exception should thrown if the XPath Query does not succesfully evaluate
        /// </summary>
        public bool ExceptionIfNotFound
        {
            get { return exceptionIfNotFound; }
            set { exceptionIfNotFound = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiate an XPathInstruction
        /// </summary>
        /// <param name="_XPathQuery">The XPath query that is to be executed</param>
        /// <param name="promotion">Whether to write or promote the result</param>
        /// <param name="_XPathResultType">Whether the resulting nodes value, name, or namespace should be considered the result</param>
        /// <param name="propertyName">The name of the context property which will be affected</param>
        /// <param name="propertyNamespace">The namespace of the context property which will be affected</param>
        /// <param name="type">The type that the value should be cast to</param>
        /// <param name="exceptionIfNotFound"></param>
        public XPathInstruction(string _XPathQuery, ContextInstructionTypeEnum promotion, XPathResultTypeEnum _XPathResultType, string propertyName, string propertyNamespace, TypeEnum type, bool exceptionIfNotFound)
        {
            this._XPathQuery = _XPathQuery;
            this.promotion = promotion;
            this._XPathResultType = _XPathResultType;
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
            this.type = type;
            this.exceptionIfNotFound = exceptionIfNotFound;
        }

        #endregion
    }
}
