using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Messaging;
using Microsoft.RuleEngine;
using Microsoft.BizTalk.Streaming;
using System.Globalization;
using Microsoft.XLANGs.BaseTypes;
using BTS;
using System.Xml;
using System.Drawing.Design;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.SampleInstructions.MetaInstructions;

namespace BREPipelineFramework.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("8559e874-8877-4687-8e20-1176f6ffee02")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public class BREPipelineFrameworkComponent : Microsoft.BizTalk.Component.Interop.IComponent, IBaseComponent, IPersistPropertyBag, IComponentUI
    {
        #region Private Properties

        private System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("BREPipelineFramework.PipelineComponents.BREPipelineFrameworkComponent", Assembly.GetExecutingAssembly());
        private bool _Enabled = true;
        private string _ApplicationContext;
        private string _ExecutionPolicy;
        private string _InstructionLoaderPolicy;
        private bool recoverableInterchangeProcessingEnabled;
        private string trackingFolder;
        private string trackingGuid;
        private XMLFactsApplicationStageEnum xmlFactsApplicationStage = XMLFactsApplicationStageEnum.AfterInstructionExecution;
        private InstructionExecutionOrderEnum instructionExecutionOrder = InstructionExecutionOrderEnum.Legacy;
        private BREPipelineMetaInstructionCollection _BREPipelineMetaInstructionCollection;
        private TypedXMLDocumentWrapper documentWrapper;
        private SQLDataConnectionCollection sqlConnectionCollection;
        private MessageUtility utility;
        private string bodyPartName = string.Empty;

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Whether this pipeline component is enabled or not, if set to false then the pipeline component will behave in a pass through manner
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
            }
        }
        
        /// <summary>
        /// The ApplicationContext which should be asserted as a string to the BRE Policy so that it can be used to selectively fire specific rules in the policy
        /// </summary>
        public string ApplicationContext
        {
            get
            {
                return _ApplicationContext;
            }
            set
            {
                _ApplicationContext = value;
            }
        }
        
        /// <summary>
        /// The name of the ExecutionPolicy to be executed
        /// </summary>
        public string ExecutionPolicy
        {
            get
            {
                return _ExecutionPolicy;
            }
            set
            {
                _ExecutionPolicy = value;
            }
        }
        
        /// <summary>
        /// The name of the InstructionLoaderPolicy to be executed
        /// </summary>
        public string InstructionLoaderPolicy
        {
            get
            {
                return _InstructionLoaderPolicy;
            }
            set
            {
                _InstructionLoaderPolicy = value;
            }
        }

        /// <summary>
        /// Whether or not Recoverable Interchange Processing is enabled
        /// </summary>
        public bool RecoverableInterchangeProcessingEnabled
        {
            get 
            { 
                return recoverableInterchangeProcessingEnabled; 
            }
            set 
            { 
                recoverableInterchangeProcessingEnabled = value;
            }
        }

        /// <summary>
        /// The folder to which tracking information should be written
        /// </summary>
        public string TrackingFolder
        {
            get 
            { 
                return trackingFolder; 
            }
            set
            {
                if (value.Length > 0)
                {
                    if (value.Substring(value.Length - 1) == @"\")
                    {
                        trackingFolder = value;
                    }
                    else
                    {
                        trackingFolder = value + @"\";
                    }
                }
                else
                {
                    trackingFolder = value;
                }
            }
        }

        /// <summary>
        /// The stage at which the stream updated by XML facts in the execution policy is used to update the message body
        /// </summary>
        public XMLFactsApplicationStageEnum XmlFactsApplicationStage
        {
            get 
            { 
                return xmlFactsApplicationStage; 
            }
            set 
            {
                xmlFactsApplicationStage = value; 
            }
        }

        /// <summary>
        /// Whether instructions should fire in legacy mode (by MetaInstruction instantiation and then rules order) or by rules order only
        /// </summary>
        public InstructionExecutionOrderEnum InstructionExecutionOrder
        {
            get 
            { 
                return instructionExecutionOrder; 
            }
            set 
            { 
                instructionExecutionOrder = value; 
            }
        }

        #endregion

        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        public string Name
        {
            get
            {
                return resourceManager.GetString("COMPONENTNAME", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Version of the component
        /// </summary>
        [Browsable(false)]
        public string Version
        {
            get
            {
                return resourceManager.GetString("COMPONENTVERSION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        
        /// <summary>
        /// Description of the component
        /// </summary>
        [Browsable(false)]
        public string Description
        {
            get
            {
                return resourceManager.GetString("COMPONENTDESCRIPTION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        #endregion
        
        #region IPersistPropertyBag members
        /// <summary>
        /// Gets class ID of component for usage from unmanaged code.
        /// </summary>
        /// <param name="classid">
        /// Class ID of the component
        /// </param>
        public void GetClassID(out System.Guid classid)
        {
            classid = new System.Guid("8559e874-8877-4687-8e20-1176f6ffee02");
        }
        
        /// <summary>
        /// not implemented
        /// </summary>
        public void InitNew()
        {
        }
        
        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="errlog">Error status</param>
        public virtual void Load(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, int errlog)
        {
            object val = null;
            val = this.ReadPropertyBag(pb, "Enabled");
            if ((val != null))
            {
                this._Enabled = ((bool)(val));
            }
            val = this.ReadPropertyBag(pb, "ApplicationContext");
            if ((val != null))
            {
                this._ApplicationContext = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "ExecutionPolicy");
            if ((val != null))
            {
                this._ExecutionPolicy = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "InstructionLoaderPolicy");
            if ((val != null))
            {
                this._InstructionLoaderPolicy = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "RecoverableInterchangeProcessingEnabled");
            if ((val != null))
            {
                this.recoverableInterchangeProcessingEnabled = ((bool)(val));
            }
            val = this.ReadPropertyBag(pb, "TrackingFolder");
            if ((val != null))
            {
                this.trackingFolder = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "XmlFactsApplicationStage");
            if ((val != null))
            {
                string enumValue = (string)val;

                if (!Enum.TryParse<XMLFactsApplicationStageEnum>(enumValue, true, out this.xmlFactsApplicationStage))
                {
                    throw new Exception(String.Format("{0} is not a valid value for XmlFactsApplicationStage.", enumValue));
                }
            }
            val = this.ReadPropertyBag(pb, "InstructionExecutionOrder");
            if ((val != null))
            {
                string enumValue = (string)val;

                if (!Enum.TryParse<InstructionExecutionOrderEnum>(enumValue, true, out this.instructionExecutionOrder))
                {
                    throw new Exception(String.Format("{0} is not a valid value for InstructionExecutionOrder.", enumValue));
                }
            }
        }
        
        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="fClearDirty">not used</param>
        /// <param name="fSaveAllProperties">not used</param>
        public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
            this.WritePropertyBag(pb, "Enabled", this.Enabled);
            this.WritePropertyBag(pb, "ApplicationContext", this.ApplicationContext);
            this.WritePropertyBag(pb, "ExecutionPolicy", this.ExecutionPolicy);
            this.WritePropertyBag(pb, "InstructionLoaderPolicy", this.InstructionLoaderPolicy);
            this.WritePropertyBag(pb, "RecoverableInterchangeProcessingEnabled", this.recoverableInterchangeProcessingEnabled);
            this.WritePropertyBag(pb, "TrackingFolder", this.trackingFolder);
            this.WritePropertyBag(pb, "XmlFactsApplicationStage", this.xmlFactsApplicationStage.ToString());
            this.WritePropertyBag(pb, "InstructionExecutionOrder", this.instructionExecutionOrder.ToString());
        }
        
        #region utility functionality
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName)
        {
            object val = null;
            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (System.ArgumentException )
            {
                return val;
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
            return val;
        }
        
        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
        }
        #endregion
        #endregion
        
        #region IComponentUI members
        /// <summary>
        /// Component icon to use in BizTalk Editor
        /// </summary>
        [Browsable(false)]
        public IntPtr Icon
        {
            get
            {
                return ((System.Drawing.Bitmap)(this.resourceManager.GetObject("COMPONENTICON", System.Globalization.CultureInfo.InvariantCulture))).GetHicon();
            }
        }
        
        /// <summary>
        /// The Validate method is called by the BizTalk Editor during the build 
        /// of a BizTalk project.
        /// </summary>
        /// <param name="obj">An Object containing the configuration properties.</param>
        /// <returns>The IEnumerator enables the caller to enumerate through a collection of strings containing error messages. These error messages appear as compiler error messages. To report successful property validation, the method should return an empty enumerator.</returns>
        public System.Collections.IEnumerator Validate(object obj)
        {
            // example implementation:
            // ArrayList errorList = new ArrayList();
            // errorList.Add("This is a compiler error");
            // return errorList.GetEnumerator();
            return null;
        }
        #endregion
        
        #region IComponent members
        /// <summary>
        /// Implements IComponent.Execute method.
        /// </summary>
        /// <param name="pc">Pipeline context</param>
        /// <param name="inmsg">Input message</param>
        /// <returns>Original input message</returns>
        /// <remarks>
        /// IComponent.Execute method is used to initiate
        /// the processing of the message in this pipeline component.
        /// </remarks>
        public Microsoft.BizTalk.Message.Interop.IBaseMessage Execute(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            if (_Enabled)
            {
                // Reset the instructionCounter, this is done just in case a thread is being reused (not sure if this is actually the way BizTalk behaves under heavy load
                // but doing this to be on the safe side) and the old value has persisted, and also in case the BREPipelineFrameworkComponent pipeline component is used
                // multiple times in the same pipeline in which case it will definitely reuse the same thread
                BREPipelineMetaInstructionCollection.instructionCounter = 0;

                // Setup the BREPipelineMetaInstructionCollection by copying over the body and context from the input message
                SetupBREPipelineMetaInstructionCollection(pc, inmsg);

                // Instantiate a TypedXMLDocumentWrapper class pointing to the body part's stream, a SQLDataConnectionCollection, and a MessageUtility object, passing in the 
                // body part's stream, and if available the message type context property value as well
                InstantiateInstructionLoaderPolicyFacts(inmsg);

                // Set ApplicationContext to String.Empty if it has a null value since that would cause the rules engine to crash
                if (_ApplicationContext == null)
                {
                    _ApplicationContext = string.Empty;
                }

                try
                {
                    if (!string.IsNullOrEmpty(_InstructionLoaderPolicy))
                    {
                        object[] instructionLoaderFacts = { _ApplicationContext, _BREPipelineMetaInstructionCollection, documentWrapper, sqlConnectionCollection, utility };
                        ExecutePolicy(_InstructionLoaderPolicy, instructionLoaderFacts);
                    }

                    // Override the default ExecutionPolicy, ApplicationContext, and XMLFactsApplicationStage if an override instruction was set by the InstructionLoaderPolicy
                    ApplyOverrides();

                    // Add out of the box MetaInstructions to the collection so they can be used in any ExecutionPolicy
                    AddOutOfTheBoxMetaInstructions();

                    // Execute the ExecutionPolicy using the instantiated MetaInstructions as facts and passing in the TypedXMLDocument from within the wrapper if properly setup
                    // as well as any DataConnections that were setup in the InstructionLoaderPolicy
                    if (!string.IsNullOrEmpty(_ExecutionPolicy))
                    {
                        object[] pipelineMetaInstructionFacts = SetupExecutionPolicyFacts();

                        // Execute the policy in question, utilizing the DebugTrackingInspector if a TrackingFolder has been specified
                        ExecutePolicy(_ExecutionPolicy, pipelineMetaInstructionFacts);

                        // If any of the MetaInstructions have returned exceptions then throw them now.
                        _BREPipelineMetaInstructionCollection.ThrowExceptions();

                        try
                        {
                            // If a TypedXMLDocument had been setup and xmlFactsApplicationStage is set to BeforeInstructionExecution then fetch 
                            // the potentially updated body from the asserted fact and replace the body with this
                            if (xmlFactsApplicationStage == XMLFactsApplicationStageEnum.BeforeInstructionExecution && documentWrapper.DocumentCount == 1)
                            {
                                TypedXMLDocumentWrapper.ApplyTypedXMLDocument((TypedXmlDocument)pipelineMetaInstructionFacts[2], _BREPipelineMetaInstructionCollection.InMsg, pc);
                            }

                            // Execute all the instructions that have been loaded into the MetaInstructions by the ExecutionPolicy
                            _BREPipelineMetaInstructionCollection.Execute();

                            // If a TypedXMLDocument had been setup and xmlFactsApplicationStage is set to AfterInstructionExecution then fetch 
                            // the potentially updated body from the asserted fact and replace the body with this.  First check to see if the 
                            // message and/or body part are null and if so create them with context copied from the original message
                            if (xmlFactsApplicationStage == XMLFactsApplicationStageEnum.AfterInstructionExecution && documentWrapper.DocumentCount == 1)
                            {
                                RecreateBizTalkMessageAndBodyPartIfNecessary(pc, inmsg);
                                TypedXMLDocumentWrapper.ApplyTypedXMLDocument((TypedXmlDocument)pipelineMetaInstructionFacts[2], _BREPipelineMetaInstructionCollection.InMsg, pc);
                            }
                        }
                        catch (Exception)
                        {
                            _BREPipelineMetaInstructionCollection.Compensate();
                            throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    // Handle any exception in a recoverable fashion if RIP has been setup, otherwise throw the exception
                    if (recoverableInterchangeProcessingEnabled)
                    {
                        _BREPipelineMetaInstructionCollection.InMsg.SetErrorInfo(e);
                        PropertyBase messageDestinationProperty = new MessageDestination();
                        _BREPipelineMetaInstructionCollection.InMsg.Context.Write(messageDestinationProperty.QName.Name, messageDestinationProperty.QName.Namespace, "SuspendQueue");
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    //Close all SQLConnections in the collection
                    if (sqlConnectionCollection.SQLConnectionCount > 0)
                    {
                        sqlConnectionCollection.CloseSQLConnections();
                    }
                }

                // If RIP has been setup then apply the appropriate context properties to ensure that routing failures are handled in a recoverable fashion
                if (recoverableInterchangeProcessingEnabled)
                {
                    PropertyBase suspendOnRoutingFailureProperty = new SuspendMessageOnRoutingFailure();
                    _BREPipelineMetaInstructionCollection.InMsg.Context.Write(suspendOnRoutingFailureProperty.QName.Name, suspendOnRoutingFailureProperty.QName.Namespace, true);
                }

                // Return the updated message
                return _BREPipelineMetaInstructionCollection.InMsg;
            }

            // Return the original message, treating this pipeline component as if it were a pass through pipeline component if the Enabled property is set to false
            return inmsg;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Setup the BREPipelineMetaInstructionCollection by copying over the body and context from the input message
        /// </summary>
        private void SetupBREPipelineMetaInstructionCollection(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            _BREPipelineMetaInstructionCollection = new BREPipelineMetaInstructionCollection(xmlFactsApplicationStage, instructionExecutionOrder);
            _BREPipelineMetaInstructionCollection.Pc = pc;

            // Get a stream containing the body part of the original message
            IBaseMessagePart msgPart = inmsg.BodyPart;
            Stream msgStream = msgPart.GetOriginalDataStream();

            // Copy the original stream to a new stream so that the original message is preserved
            VirtualStream newMsgStream = new VirtualStream();
            msgStream.CopyTo(newMsgStream);
            newMsgStream.Seek(0, SeekOrigin.Begin);

            // Create a copy of the original body part and copy over it's properties as well
            IBaseMessagePart copiedBodyPart = pc.GetMessageFactory().CreateMessagePart();
            copiedBodyPart.Data = newMsgStream;
            copiedBodyPart.PartProperties = inmsg.BodyPart.PartProperties;

            // Create a new message in the _BREPipelineMetaInstructionCollection and copy over the body and any additional parts
            _BREPipelineMetaInstructionCollection.InMsg = pc.GetMessageFactory().CreateMessage();
            CopyMessageParts(inmsg, _BREPipelineMetaInstructionCollection.InMsg, copiedBodyPart);

            // Add the new message body part's stream to the Pipeline Context's resource tracker so that it will be disposed off correctly
            pc.ResourceTracker.AddResource(newMsgStream);

            // Create and copy over context from the original message onto the message in the context manipulator collection
            _BREPipelineMetaInstructionCollection.InMsg.Context = pc.GetMessageFactory().CreateMessageContext();

            // Iterate through inbound message context properties and add to the new outbound message
            CopyMessageContext(inmsg);
        }

        /// <summary>
        /// Copy over all parts of the message from a source message to a destination message, choosing which part should be the body part
        /// </summary>
        private void CopyMessageParts(IBaseMessage sourceMessage, IBaseMessage destinationMessage, IBaseMessagePart newBodyPart)
        {
            string bodyPartName = sourceMessage.BodyPartName;
            for (int c = 0; c < sourceMessage.PartCount; ++c)
            {
                string partName = null;
                IBaseMessagePart messagePart = sourceMessage.GetPartByIndex(c, out partName);
                if (partName != bodyPartName)
                {
                    destinationMessage.AddPart(partName, messagePart, false);
                }
                else
                {
                    destinationMessage.AddPart(bodyPartName, newBodyPart, true);
                }
            }
        }

        /// <summary>
        /// Copy message context from the input BizTalk message to the BizTalk message in the _BREPipelineMetaInstructionCollection
        /// </summary>
        private void CopyMessageContext(Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            for (int contextCounter = 0; contextCounter < inmsg.Context.CountProperties; contextCounter++)
            {
                string Name;
                string Namespace;

                object PropertyValue = inmsg.Context.ReadAt(contextCounter, out Name, out Namespace);

                // If the property has been promoted, respect the settings
                if (inmsg.Context.IsPromoted(Name, Namespace))
                {
                    _BREPipelineMetaInstructionCollection.InMsg.Context.Promote(Name, Namespace, PropertyValue);
                }
                else
                {
                    _BREPipelineMetaInstructionCollection.InMsg.Context.Write(Name, Namespace, PropertyValue);
                }
            }
        }

        /// <summary>
        /// Instantiate a TypedXMLDocumentWrapper class pointing to the body part's stream, a SQLDataConnectionCollection, and a MessageUtility object, passing in the 
        /// body part's stream, and if available the message type context property value as well
        /// </summary>
        private void InstantiateInstructionLoaderPolicyFacts(Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            documentWrapper = new TypedXMLDocumentWrapper(_BREPipelineMetaInstructionCollection.InMsg.BodyPart.Data);
            sqlConnectionCollection = new SQLDataConnectionCollection();

            try
            {
                string messageType = inmsg.Context.Read("MessageType", ContextPropertyNamespaces._BTSPropertyNamespace).ToString();
                utility = new MessageUtility(_BREPipelineMetaInstructionCollection.InMsg.BodyPart.Data, messageType);
            }
            catch
            {
                utility = new MessageUtility(_BREPipelineMetaInstructionCollection.InMsg.BodyPart.Data);
            }
        }

        /// <summary>
        /// Execute the policy in question, utilizing the DebutTrackingInspector if a TrackingFolder has been specified
        /// </summary>
        private void ExecutePolicy(string policyName, object[] facts)
        {
            using (Microsoft.RuleEngine.Policy policy = new Policy(policyName))
            {
                if (string.IsNullOrEmpty(trackingFolder))
                {
                    policy.Execute(facts);
                }
                else
                {
                    if (trackingFolder.Substring(trackingFolder.Length - 1) != @"\")
                    {
                        trackingFolder = trackingFolder + @"\";
                    }

                    if (!Directory.Exists(trackingFolder))
                    {
                        throw new Exception(String.Format("Tracking folder {0} does not resolve to a valid folder location", trackingFolder));
                    }

                    if (string.IsNullOrEmpty(trackingGuid))
                    {
                        trackingGuid = Guid.NewGuid().ToString();
                    }

                    DebugTrackingInterceptor dti = new DebugTrackingInterceptor(trackingFolder + String.Format("{0}.{1}.{2}-{3}.txt",
                        policy.PolicyName, policy.MajorRevision.ToString(), policy.MinorRevision.ToString(), trackingGuid));

                    try
                    {
                        policy.Execute(facts, dti);
                    }
                    catch (PolicyExecutionException ex)
                    {
                        string exceptionMessage = string.Format("Exception encountered while executing BRE policy {0}.  Top level exception message - {1}", policyName, ex.InnerException.Message);

                        if (ex.InnerException.InnerException != null)
                        {
                            exceptionMessage = exceptionMessage + Environment.NewLine;
                            exceptionMessage = exceptionMessage + string.Format("Innermost exception was - {0}", ex.GetBaseException());
                        }

                        throw new Exception(exceptionMessage);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        dti.CloseTraceFile();
                    }
                }
            }
        }

        /// <summary>
        /// Override the default ExecutionPolicy and ApplicationContext if an override instruction was set by the InstructionLoaderPolicy
        /// </summary>
        private void ApplyOverrides()
        {
            // Override the Execution Policy if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (!String.IsNullOrEmpty(_BREPipelineMetaInstructionCollection.ExecutionPolicyOverride))
            {
                _ExecutionPolicy = _BREPipelineMetaInstructionCollection.ExecutionPolicyOverride;
            }

            // Override the Application Context if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (!String.IsNullOrEmpty(_BREPipelineMetaInstructionCollection.ApplicationContextOverride))
            {
                _ApplicationContext = _BREPipelineMetaInstructionCollection.ApplicationContextOverride;
            }

            // Override the XML Facts application stage if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (_BREPipelineMetaInstructionCollection.XmlFactsApplicationStageOverride != xmlFactsApplicationStage)
            {
                xmlFactsApplicationStage = _BREPipelineMetaInstructionCollection.XmlFactsApplicationStageOverride;
            }
        }
                
        /// <summary>
        /// If the BizTalk message in the _BREPipelineMetaInstructionCollection and/or it's body part are null then instantiate them and copy
        /// context over from the original message
        /// </summary>
        private void RecreateBizTalkMessageAndBodyPartIfNecessary(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            // If the BizTalk message has been nullified then create the message again and copy over the context from the original message
            if (_BREPipelineMetaInstructionCollection.InMsg == null)
            {
                _BREPipelineMetaInstructionCollection.InMsg = pc.GetMessageFactory().CreateMessage();
                CopyMessageContext(inmsg);
            }

            // If the message body part is null then instantiate it again so we can assign the typed xml document stream to it
            if (_BREPipelineMetaInstructionCollection.InMsg.BodyPart == null)
            {
                IBaseMessagePart messageBodyPart = pc.GetMessageFactory().CreateMessagePart();
                _BREPipelineMetaInstructionCollection.InMsg.AddPart("Body", messageBodyPart, true);
            }
        }

        /// <summary>
        /// Add out of the box MetaInstructions to the collection so they can be used in any ExecutionPolicy
        /// </summary>
        private void AddOutOfTheBoxMetaInstructions()
        {
            // Add the out of the box MetaInstruction classes to the MetaInstrumentCollection
            ContextMetaInstructions contextMetaInstruction = new ContextMetaInstructions();
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(contextMetaInstruction);
            HelperMetaInstructions helperMetaInstruction = new HelperMetaInstructions();
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(helperMetaInstruction);
            CachingMetaInstructions cachingMetaInstruction = new CachingMetaInstructions();
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(cachingMetaInstruction);

            // Only add TypedXMLDocumentMetaInstructions if a TypedXMLDocument has been setup
            if (documentWrapper.DocumentCount == 1)
            {
                TypedXMLDocumentMetaInstructions xmlInstructions = new TypedXMLDocumentMetaInstructions(documentWrapper.Document, xmlFactsApplicationStage);
                _BREPipelineMetaInstructionCollection.AddMetaInstruction(xmlInstructions);
            }
        }

        /// <summary>
        /// Setup the ExecutionPolicy with the appropriate facts
        /// </summary>
        private object[] SetupExecutionPolicyFacts()
        {
            object[] pipelineMetaInstructionFacts = new object[2 + documentWrapper.DocumentCount + sqlConnectionCollection.SQLConnectionCount + _BREPipelineMetaInstructionCollection.GetCount()];
            pipelineMetaInstructionFacts[0] = _ApplicationContext;
            pipelineMetaInstructionFacts[1] = _BREPipelineMetaInstructionCollection.Pc;

            // Add the TypedXmlDocument to the fact array if it has been setup
            if (documentWrapper.DocumentCount == 1)
            {
                pipelineMetaInstructionFacts[2] = documentWrapper.Document;
            }

            // Add any SQL DataConnection that were added to by the InstructionLoaderPolicy to the fact array
            for (int i = 1; i <= sqlConnectionCollection.SQLConnectionCount; i++)
            {
                pipelineMetaInstructionFacts[i + 1 + documentWrapper.DocumentCount] = sqlConnectionCollection.GetDataConnectionByIndex(i - 1);
            }

            // Add the out of the box MetaInstructions and any other added to the MetaInstructionCollection by the InstructionLoaderPolicy to the fact array
            for (int i = 1; i <= _BREPipelineMetaInstructionCollection.GetCount(); i++)
            {
                pipelineMetaInstructionFacts[i + 1 + documentWrapper.DocumentCount + sqlConnectionCollection.SQLConnectionCount] = _BREPipelineMetaInstructionCollection.GetMetaInstructionByIndex(i - 1);
            }

            return pipelineMetaInstructionFacts;
        }
        
        #endregion
    }
}
