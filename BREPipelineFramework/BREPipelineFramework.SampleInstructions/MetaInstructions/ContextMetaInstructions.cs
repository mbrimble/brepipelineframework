using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using System.Reflection;
using BREPipelineFramework.SampleInstructions.Instructions;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class ContextMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private Properties

        private SetContextPropertyFromXPathResultPipelineInstruction setContextPropertyFromXPathResultPipelineInstruction = null;

        #endregion

        #region Private Constants

        private const string _BTSPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
        private const string _ErrorReportPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2005/error-report";
        private const string _FILEPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/file-properties";
        private const string _FTPPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/ftp-properties";
        private const string _HTTPPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/http-properties";
        private const string _LEGACYPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/legacy-properties";
        private const string _MessageTrackingPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/messagetracking-properties";
        private const string _MIMEPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/mime-properties";
        private const string _MSMQTPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/msmqt-properties";
        private const string _POP3PropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/pop3-properties";
        private const string _SMTPPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/smtp-properties";
        private const string _SOAPPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/soap-properties";
        private const string _SQLPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/sql-properties";
        private const string _SharePointPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/WindowsSharePointServices-properties";
        private const string _XMLNormPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties";
        private const string _XLANGSPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/xlangs-properties";
        private const string _WCFPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties";
        private const string _BTF2PropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/btf2-properties";
        private const string _EDIPropertyNamespace = "http://schemas.microsoft.com/Edi/PropertySchema";
        private const string _EDIAS2PropertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/as2-properties";
        private const string _EDIOverridePropertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/edi-properties";
        private const string _SBMessagingPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2012/Adapter/BrokeredMessage-properties";
        private const string _SFTPPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2012/Adapter/sftp-properties";

        #endregion

        #region Public Instruction Setup Methods

        /// <summary>
        /// Setup an Instruction that will remove a context property from a message
        /// </summary>
        /// <param name="propertyName">The name of the context property to remove</param>
        /// <param name="propertyNamespace">The namespace of the context property to remove</param>
        public void RemoveContextProperty(string propertyName, string propertyNamespace)
        {
            RemoveContextPropertyPipelineInstruction instruction = new RemoveContextPropertyPipelineInstruction(propertyName, propertyNamespace);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within any namespace
        /// </summary>
        /// <param name="propertyName">The name of the context property</param>
        /// <param name="propertyNamespace">The namespace of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetCustomContextProperty(string propertyName, string propertyNamespace, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetContextPropertyPipelineInstruction instruction = new SetContextPropertyPipelineInstruction(propertyName, propertyNamespace, value, promotion, type);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the BTS namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetGlobalPropertySchemasContextProperty(BizTalkGlobalPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _BTSPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the ErrorReport namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetErrorReportContextProperty(BizTalkErrorReportPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _ErrorReportPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the FILE namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetFILEContextProperty(BizTalkFilePropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _FILEPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the FTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetFTPContextProperty(BizTalkFTPPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _FTPPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the HTTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetHTTPContextProperty(BizTalkHTTPPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _HTTPPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the LEGACY namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetLEGACYContextProperty(BizTalkLegacyPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _LEGACYPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the MessageTracking namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetMessageTrackingContextProperty(BizTalkMessageTrackingPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _MessageTrackingPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the MIME namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetMIMETrackingContextProperty(BizTalkMIMEPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _MIMEPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the MSMQT namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetMSMQTContextProperty(BizTalkMSMQTPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _MSMQTPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the POP3 namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetPOP3ContextProperty(BizTalkPOP3PropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _POP3PropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the SMTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetSMTPContextProperty(BizTalkSMTPPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _SMTPPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the SOAP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetSOAPContextProperty(BizTalkSOAPPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _SOAPPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the SQL namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetSQLContextProperty(BizTalkSQLPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _SQLPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the WSS namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetWSSContextProperty(BizTalkWSSPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _SharePointPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the XMLNORM namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetXMLNORMContextProperty(BizTalkXMLNORMPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _XMLNormPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the Microsoft.BizTalk.XLANGs.BTXEngine namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetXLANGsBTXEngineContextProperty(BizTalkXLANGSBTXEnginePropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _XLANGSPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the WCF namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetWCFPropertySchemasContextProperty(BizTalkWCFPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _WCFPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the BTF2 namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetBTF2PropertySchemasContextProperty(BizTalkBTF2PropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _BTF2PropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the EDI namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetEDIPropertySchemasContextProperty(BizTalkEDIPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _EDIPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the EdiIntAS namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetEdiIntASPropertySchemasContextProperty(BizTalkEdiIntASPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _EDIAS2PropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the EdiOverride namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetEdiOverridePropertySchemasContextProperty(BizTalkEdiOverridePropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _EDIOverridePropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the SFTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetSFTPPropertySchemasContextProperty(BizTalkSFTPPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _SFTPPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified value to a context property within the SBMessaging namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="value">The value to be written/promoted to the context</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetSBMessagingPropertySchemasContextProperty(BizTalkSBMessagingPropertySchemaEnum property, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            SetCustomContextProperty(property.ToString(), _SBMessagingPropertyNamespace, value, promotion, type);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified a value read in from the SSO Database to a context property within any namespace
        /// </summary>
        /// <param name="propertyName">The name of the context property</param>
        /// <param name="propertyNamespace">The namespace of the context property</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="_SSOApplication">The name of the SSO Application</param>
        /// <param name="_SSOKey">The name of the SSO Key in the key/value pair</param>
        /// <param name="type">The type to cast the value to</param>
        public void SetContextPropertyFromSSOConfig(string propertyName, string propertyNamespace, ContextInstructionTypeEnum promotion, string _SSOApplication, string _SSOKey, TypeEnum type)
        {
            SetContextPropertyFromSSOConfigPipelineInstruction instruction = new SetContextPropertyFromSSOConfigPipelineInstruction(propertyName, propertyNamespace, promotion, _SSOApplication, _SSOKey, type);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Setup an Instruction that will either promote or write a specified a value that is the result of an XPath Expression to a context property within any namespace
        /// </summary>
        /// <param name="propertyName">The name of the context property</param>
        /// <param name="propertyNamespace">The namespace of the context property</param>
        /// <param name="promotion">Whether to write or promote the context property</param>
        /// <param name="_XPathResultType">Whether the resulting node's value, name, or namspace should be treated as the result</param>
        /// <param name="_XPathQuery">The XPath Expression</param>
        /// <param name="type">The type to cast the value to</param>
        /// <param name="exceptionIfNotFound">Whether or not to thrown an exception if the XPath expression does not evaluate</param>
        public void SetContextPropertyFromXPathResult(string propertyName, string propertyNamespace, ContextInstructionTypeEnum promotion, XPathResultTypeEnum _XPathResultType, string _XPathQuery, TypeEnum type, bool exceptionIfNotFound)
        {
            if (setContextPropertyFromXPathResultPipelineInstruction == null)
            {
                setContextPropertyFromXPathResultPipelineInstruction = new SetContextPropertyFromXPathResultPipelineInstruction();
                base.AddInstruction(setContextPropertyFromXPathResultPipelineInstruction);
            }

            setContextPropertyFromXPathResultPipelineInstruction.AddXPathInstruction(new XPathInstruction(_XPathQuery, promotion, _XPathResultType, propertyName, propertyNamespace, type, exceptionIfNotFound));
        }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Get the object value of a context property in any namespace
        /// </summary>
        /// <param name="Name">The name of the context property</param>
        /// <param name="Namespace">The namespace of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetContextProperty(string Name, string Namespace, FailureActionEnum failureAction)
        {
            string property = "";

            try
            {
                property = base.InMsg.Context.Read(Name, Namespace).ToString();
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property " + Name + " in namespace " + Namespace + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }
            return property;
        }

        /// <summary>
        /// Get the object value of a context property in the BTS namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetGlobalPropertySchemasContextProperty(BizTalkGlobalPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _BTSPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property BTS." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the WCF namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetWCFPropertySchemasContextProperty(BizTalkWCFPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _WCFPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property WCF." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the BTF2 namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetBTF2PropertySchemasContextProperty(BizTalkBTF2PropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _BTF2PropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property BTF2." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the ErrorReport namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetErrorReportPropertySchemasContextProperty(BizTalkErrorReportPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _ErrorReportPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property ErrorReport." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the FILE namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetFILEPropertySchemasContextProperty(BizTalkFilePropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _FILEPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property FILE." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the FTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetFTPPropertySchemasContextProperty(BizTalkFTPPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _FTPPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property FTP." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the HTTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetHTTPPropertySchemasContextProperty(BizTalkHTTPPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _HTTPPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property HTTP." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the LEGACY namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetLEGACYPropertySchemasContextProperty(BizTalkLegacyPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _LEGACYPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property LEGACY." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the MessageTracking namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetMessageTrackingPropertySchemasContextProperty(BizTalkMessageTrackingPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _MessageTrackingPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property MessageTracking." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the MIME namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetMIMEPropertySchemasContextProperty(BizTalkMIMEPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _MIMEPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property MIME." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the MSMQT namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetMSMQTPropertySchemasContextProperty(BizTalkMSMQTPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _MSMQTPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property MSMQT." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the POP3 namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetPOP3PropertySchemasContextProperty(BizTalkPOP3PropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _POP3PropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property POP3." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }


        /// <summary>
        /// Get the object value of a context property in the SMTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetSMTPPropertySchemasContextProperty(BizTalkSMTPPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _SMTPPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property SMTP." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the SOAP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetSOAPPropertySchemasContextProperty(BizTalkSOAPPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _SOAPPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property SOAP." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the SQL namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetSQLPropertySchemasContextProperty(BizTalkSQLPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _SQLPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property SQL." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the WSS namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetWSSPropertySchemasContextProperty(BizTalkWSSPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _SharePointPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property WSS." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the Microsoft.BizTalk.XLANGs.BTXEngine namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetXLANGSBTXEnginePropertySchemasContextProperty(BizTalkXLANGSBTXEnginePropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _XLANGSPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property Microsoft.BizTalk.XLANGs.BTXEngine." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the XMLNORM namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetXMLNORMPropertySchemasContextProperty(BizTalkXMLNORMPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _XMLNormPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property XMLNORM." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the EDI namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetEDIPropertySchemasContextProperty(BizTalkEDIPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _EDIPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property EDI." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the EdiIntAS namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetEdiIntASPropertySchemasContextProperty(BizTalkEdiIntASPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _EDIAS2PropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property EdiIntAS." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the EdiOverride namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetEdiOverridePropertySchemasContextProperty(BizTalkEdiOverridePropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _EDIOverridePropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property EdiOverride." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the SFTP namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetSFTPPropertySchemasContextProperty(BizTalkSFTPPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _SFTPPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property SFTP." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Get the object value of a context property in the SBMessaging namespace
        /// </summary>
        /// <param name="property">The name of the context property</param>
        /// <param name="failureAction">Whether to throw an exception or return a blank string if the property is not found</param>
        /// <returns>The object value of the context property</returns>
        public string GetSBMessagingPropertySchemasContextProperty(BizTalkSBMessagingPropertySchemaEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = GetContextProperty(property.ToString(), _SBMessagingPropertyNamespace, failureAction);
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get context property SBMessaging." + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        /// <summary>
        /// Create an SSO ticket so that credentials on a compatible adapter can be read in from an SSO credential store
        /// </summary>
        public void CreateSSOTicket()
        {
            CreateSSOTicketInstruction instruction = new CreateSSOTicketInstruction();
            base.AddInstruction(instruction);
        }

        #endregion
    }
}
