using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.RuleEngine;
using Microsoft.BizTalk.Streaming;
using Microsoft.XLANGs.BaseTypes;
using BTS;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.SampleInstructions.MetaInstructions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BREPipelineFramework.Helpers.Tracing;
using BREPipelineFramework.SampleInstructions;
using Microsoft.BizTalk.Component;

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
        private string applicationContext;
        private string executionPolicy;
        private string instructionLoaderPolicy;
        private bool recoverableInterchangeProcessingEnabled;
        private string trackingFolder;
        private string callToken;
        private XMLFactsApplicationStageEnum _XMLFactsApplicationStage = XMLFactsApplicationStageEnum.BeforeInstructionExecution;
        private InstructionExecutionOrderEnum instructionExecutionOrder = InstructionExecutionOrderEnum.RulesExecution;
        private BREPipelineMetaInstructionCollection _BREPipelineMetaInstructionCollection;
        private TypedXMLDocumentWrapper documentWrapper;
        private SQLDataConnectionCollection sqlConnectionCollection;
        private MessageUtility utility;
        private string instructionLoaderPolicyVersion;
        private string executionPolicyVersion;
        private Dictionary<int, string> partNames = new Dictionary<int, string>();
        private string streamsToReadBeforeExecution = "Microsoft.BizTalk.Component.XmlDasmStreamWrapper";
        private Stream originalStream;

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Whether this pipeline component is enabled or not, if set to false then the pipeline component will behave in a pass through manner
        /// </summary>
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public string ApplicationContext
        {
            get
            {
                return applicationContext;
            }
            set
            {
                applicationContext = value;
            }
        }
        
        /// <summary>
        /// The name of the ExecutionPolicy to be executed
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string ExecutionPolicy
        {
            get
            {
                return executionPolicy;
            }
            set
            {
                executionPolicy = value;
            }
        }
        
        /// <summary>
        /// The name of the InstructionLoaderPolicy to be executed
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string InstructionLoaderPolicy
        {
            get
            {
                return instructionLoaderPolicy;
            }
            set
            {
                instructionLoaderPolicy = value;
            }
        }

        /// <summary>
        /// Whether or not Recoverable Interchange Processing is enabled
        /// </summary>
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public XMLFactsApplicationStageEnum XmlFactsApplicationStage
        {
            get 
            { 
                return _XMLFactsApplicationStage; 
            }
            set 
            {
                _XMLFactsApplicationStage = value; 
            }
        }

        /// <summary>
        /// Whether instructions should fire in legacy mode (by MetaInstruction instantiation and then rules order) or by rules order only
        /// </summary>
        [ExcludeFromCodeCoverage]
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

        /// <summary>
        /// Allows the ExecutionPolicy version number to be optionally specified
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string ExecutionPolicyVersion
        {
            get 
            { 
                return executionPolicyVersion; 
            }
            set 
            { 
                executionPolicyVersion = value; 
            }
        }

        /// <summary>
        /// Allows the InstructionLoaderPolicy version number to be optionally specified
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string InstructionLoaderPolicyVersion
        {
            get 
            { 
                return instructionLoaderPolicyVersion; 
            }
            set 
            { 
                instructionLoaderPolicyVersion = value; 
            }
        }

        /// <summary>
        /// A comma seperated list of string values denoting stream types that should be read before the pipeline component is executed
        /// </summary>
        [ExcludeFromCodeCoverage]
        public string StreamsToReadBeforeExecution
        {
            get
            { 
                return streamsToReadBeforeExecution; 
            }
            set 
            { 
                streamsToReadBeforeExecution = value; 
            }
        }


        #endregion

        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>
        [Browsable(false)]
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
        public void GetClassID(out System.Guid classid)
        {
            classid = new System.Guid("8559e874-8877-4687-8e20-1176f6ffee02");
        }
        
        /// <summary>
        /// not implemented
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void InitNew()
        {
        }
        
        /// <summary>
        /// Loads configuration properties for the component
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="errlog">Error status</param>
        [ExcludeFromCodeCoverage]
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
                this.applicationContext = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "ExecutionPolicy");
            if ((val != null))
            {
                this.executionPolicy = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "InstructionLoaderPolicy");
            if ((val != null))
            {
                this.instructionLoaderPolicy = ((string)(val));
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

                if (!Enum.TryParse<XMLFactsApplicationStageEnum>(enumValue, true, out this._XMLFactsApplicationStage))
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
            val = this.ReadPropertyBag(pb, "ExecutionPolicyVersion");
            if ((val != null))
            {
                this.executionPolicyVersion = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "InstructionLoaderPolicyVersion");
            if ((val != null))
            {
                this.instructionLoaderPolicyVersion = ((string)(val));
            }
            val = this.ReadPropertyBag(pb, "StreamsToReadBeforeExecution");
            if ((val != null))
            {
                this.streamsToReadBeforeExecution = ((string)(val));
            }
        }
        
        /// <summary>
        /// Saves the current component configuration into the property bag
        /// </summary>
        /// <param name="pb">Configuration property bag</param>
        /// <param name="fClearDirty">not used</param>
        /// <param name="fSaveAllProperties">not used</param>
        [ExcludeFromCodeCoverage]
        public virtual void Save(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, bool fClearDirty, bool fSaveAllProperties)
        {
            this.WritePropertyBag(pb, "Enabled", this.Enabled);
            this.WritePropertyBag(pb, "ApplicationContext", this.ApplicationContext);
            this.WritePropertyBag(pb, "ExecutionPolicy", this.ExecutionPolicy);
            this.WritePropertyBag(pb, "InstructionLoaderPolicy", this.InstructionLoaderPolicy);
            this.WritePropertyBag(pb, "RecoverableInterchangeProcessingEnabled", this.recoverableInterchangeProcessingEnabled);
            this.WritePropertyBag(pb, "TrackingFolder", this.trackingFolder);
            this.WritePropertyBag(pb, "XmlFactsApplicationStage", this._XMLFactsApplicationStage.ToString());
            this.WritePropertyBag(pb, "InstructionExecutionOrder", this.instructionExecutionOrder.ToString());
            this.WritePropertyBag(pb, "ExecutionPolicyVersion", this.executionPolicyVersion);
            this.WritePropertyBag(pb, "InstructionLoaderPolicyVersion", this.instructionLoaderPolicyVersion);
            this.WritePropertyBag(pb, "StreamsToReadBeforeExecution", this.streamsToReadBeforeExecution);
        }
        
        #region utility functionality
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
            long ticks = TraceIn();

            if (_Enabled)
            {
                // Setup the BREPipelineMetaInstructionCollection by copying over the body and context from the input message
                SetupBREPipelineMetaInstructionCollection(pc, inmsg);

                // Instantiate a TypedXMLDocumentWrapper class pointing to the body part's stream, a SQLDataConnectionCollection, and a MessageUtility object, passing in the 
                // body part's stream, and if available the message type context property value as well
                InstantiateInstructionLoaderPolicyFacts(pc);

                // Set ApplicationContext to String.Empty if it has a null value since that would cause the rules engine to crash
                if (applicationContext == null)
                {
                    applicationContext = string.Empty;
                }

                try
                {
                    // Execute the InstructionLoaderPolicy
                    if (!string.IsNullOrEmpty(instructionLoaderPolicy))
                    {
                        object[] instructionLoaderFacts = { applicationContext, _BREPipelineMetaInstructionCollection, documentWrapper, sqlConnectionCollection, utility };
                        ExecutePolicy(instructionLoaderPolicy, instructionLoaderFacts, instructionLoaderPolicyVersion);
                    }

                    // If the InstructionLoaderPolicy returned a handled error then throw that now
                    if (_BREPipelineMetaInstructionCollection.BREException != null)
                    {
                        throw _BREPipelineMetaInstructionCollection.BREException;
                    }

                    // Override the default ExecutionPolicy name and version, ApplicationContext, and XMLFactsApplicationStage if an override instruction was set by the InstructionLoaderPolicy
                    ApplyOverrides();

                    // Add out of the box MetaInstructions to the collection so they can be used in any ExecutionPolicy
                    AddOutOfTheBoxMetaInstructions();

                    // Execute the ExecutionPolicy using the instantiated MetaInstructions as facts and passing in the TypedXMLDocument from within the wrapper if properly setup
                    // as well as any DataConnections that were setup in the InstructionLoaderPolicy
                    if (!string.IsNullOrEmpty(executionPolicy))
                    {
                        object[] pipelineMetaInstructionFacts = SetupExecutionPolicyFacts();

                        // Execute the Execution policy in question, utilizing the DebugTrackingInspector if a TrackingFolder has been specified
                        ExecutePolicy(executionPolicy, pipelineMetaInstructionFacts, executionPolicyVersion);

                        // If any of the MetaInstructions have returned exceptions then throw them now.
                        _BREPipelineMetaInstructionCollection.ThrowExceptions();

                        try
                        {
                            // If a TypedXMLDocument had been setup and xmlFactsApplicationStage is set to BeforeInstructionExecution then fetch 
                            // the potentially updated body from the asserted fact and replace the message body stream with this
                            if (_XMLFactsApplicationStage == XMLFactsApplicationStageEnum.BeforeInstructionExecution && documentWrapper.DocumentCount == 1)
                            {
                                TypedXMLDocumentWrapper.ApplyTypedXMLDocument((TypedXmlDocument)pipelineMetaInstructionFacts[2], _BREPipelineMetaInstructionCollection.InMsg, pc, callToken);
                            }

                            // Execute all the instructions that have been loaded into the MetaInstructions by the ExecutionPolicy
                            _BREPipelineMetaInstructionCollection.Execute();

                            // If a TypedXMLDocument had been setup and xmlFactsApplicationStage is set to AfterInstructionExecution then fetch 
                            // the potentially updated body from the asserted fact and replace the message body stream with this.  First check to see if the 
                            // message and/or body part are null and if so create them with context copied from the original message
                            if (_XMLFactsApplicationStage == XMLFactsApplicationStageEnum.AfterInstructionExecution && documentWrapper.DocumentCount == 1)
                            {
                                RecreateBizTalkMessageAndBodyPartIfNecessary(pc, inmsg);
                                TypedXMLDocumentWrapper.ApplyTypedXMLDocument((TypedXmlDocument)pipelineMetaInstructionFacts[2], _BREPipelineMetaInstructionCollection.InMsg, pc, callToken);
                            }
                        }
                        catch (Exception)
                        {
                            // Call compensation methods on all MetaInstructions before throwing the error
                            _BREPipelineMetaInstructionCollection.Compensate();
                            throw;
                        }
                    }
                }
                catch (Exception e)
                {
                    TraceManager.PipelineComponent.TraceError(e, true, Guid.Parse(callToken));
                    
                    // Handle any exception in a recoverable fashion if RIP has been setup, otherwise throw the exception
                    if (recoverableInterchangeProcessingEnabled)
                    {
                        HandleRIPException(inmsg, e);
                    }
                    else
                    {
                        _BREPipelineMetaInstructionCollection.InMsg.BodyPart.Data.Position = 0;

                        TraceManager.PipelineComponent.TraceEndScope(callToken, ticks);
                        TraceManager.PipelineComponent.TraceOut(callToken);
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
                    _BREPipelineMetaInstructionCollection.InMsg.Context.Write(BizTalkGlobalPropertySchemaEnum.SuspendMessageOnRoutingFailure.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace,
                            true);
                }

                TraceManager.PipelineComponent.TraceEndScope(callToken, ticks);
                TraceManager.PipelineComponent.TraceOut(callToken);

                // Return the updated message
                return _BREPipelineMetaInstructionCollection.InMsg;
            }

            TraceManager.PipelineComponent.TraceEndScope(callToken, ticks);
            TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component was disabled so acting like a pass through pipeline component.",callToken);
            TraceManager.PipelineComponent.TraceOut(callToken);

            // Return the original message, treating this pipeline component as if it were a pass through pipeline component if the Enabled property is set to false
            return inmsg;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create a callToken tracing GUID for use in all trace statements for this pipeline instance and in rule engine debug tracing file names
        /// and trace input paramters
        /// </summary>
        private long TraceIn()
        {
            callToken = TraceManager.PipelineComponent.TraceIn().ToString();
            long ticks = TraceManager.PipelineComponent.TraceStartScope(callToken);
           
            TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has started executing with an application context of {1}" +
                ", an Instruction Execution Order of {2} and an XML Facts Application Stage of {3}.", callToken, applicationContext, instructionExecutionOrder, _XMLFactsApplicationStage);

            if (!string.IsNullOrEmpty(instructionLoaderPolicy))
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has an optional Instruction Loader policy paramater value set to {1}.",
                        callToken, instructionLoaderPolicy);

                if (!string.IsNullOrEmpty(instructionLoaderPolicyVersion))
                {
                    TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has an optional InstructionLoader policy version paramater value set to {1}.",
                        callToken, executionPolicyVersion);
                }
            }

            if (!string.IsNullOrEmpty(executionPolicy))
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has an optional Execution policy paramater value set to {1}.",
                        callToken, executionPolicy);

                if (!string.IsNullOrEmpty(executionPolicyVersion))
                {
                    TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has an optional Execution policy version paramater value set to {1}.",
                        callToken, executionPolicyVersion);
                }
            }

            if (recoverableInterchangeProcessingEnabled)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has Recoverable Interchange Processing enabled.", callToken);
            }

            if (!string.IsNullOrEmpty(trackingFolder))
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - BRE Pipeline Framework pipeline component has an optional tracking folder paramater value set to {1}.",
                    callToken, trackingFolder);
            }

            return ticks;
        }

        /// <summary>
        /// Setup the BREPipelineMetaInstructionCollection by copying over the body and context from the input message
        /// </summary>
        private void SetupBREPipelineMetaInstructionCollection(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg)
        {
            _BREPipelineMetaInstructionCollection = new BREPipelineMetaInstructionCollection(_XMLFactsApplicationStage, instructionExecutionOrder, partNames, callToken);
            _BREPipelineMetaInstructionCollection.Pc = pc;

            // Create a copy of the original body part and copy over it's properties as well
            IBaseMessagePart copiedBodyPart = pc.GetMessageFactory().CreateMessagePart();

            string streamType = inmsg.BodyPart.Data.GetType().ToString();
            TraceManager.PipelineComponent.TraceInfo("{0} - Inbound message body had a stream type of {1}", callToken, streamType);

            // If the input stream is not seekable then wrap it with a ReadOnlySeekableStream so that it can have it's position set
            if (!inmsg.BodyPart.GetOriginalDataStream().CanSeek)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Inbound message body stream was not seekable so wrapping it with a ReadOnlySeekableStream", callToken);

                ReadOnlySeekableStream seekableDataStream = new ReadOnlySeekableStream(inmsg.BodyPart.GetOriginalDataStream());
                originalStream = seekableDataStream;
                pc.ResourceTracker.AddResource(seekableDataStream);
            }
            else
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Inbound message body stream was seekable so no need to wrap it", callToken);

                originalStream = inmsg.BodyPart.GetOriginalDataStream();
            }

            pc.ResourceTracker.AddResource(originalStream);
            copiedBodyPart.Data = originalStream;
            ReadStreamIfNecessary(pc, inmsg, copiedBodyPart, streamType);

            //Copy over part properties
            copiedBodyPart.PartProperties = inmsg.BodyPart.PartProperties;

            // Create a new message in the _BREPipelineMetaInstructionCollection and copy over the body and any additional parts
            _BREPipelineMetaInstructionCollection.InMsg = pc.GetMessageFactory().CreateMessage();
            CopyMessageParts(inmsg, _BREPipelineMetaInstructionCollection.InMsg, copiedBodyPart);

            // Copy over context by reference, this is to ensure that context isn't lost if using an XML Disassembler prior to BRE component
            // and not reading the stream prior to cloning context
            _BREPipelineMetaInstructionCollection.InMsg.Context = inmsg.Context;
        }

        //Get the list of stream types that require reading before the pipeline executes from the pipeline parameters and read a character from the stream
        //to ensure that the stream reading logic gets exercised (especially important if the stream in question writes to the message context such as in 
        //the case of the Microsoft.BizTalk.Component.XmlDasmStreamWrapper)
        private void ReadStreamIfNecessary(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, IBaseMessagePart copiedBodyPart, string streamType)
        {
            if (!string.IsNullOrEmpty(streamsToReadBeforeExecution))
            {
                streamsToReadBeforeExecution.Replace(" ", "");
                List<string> streamsToReadBeforeExecutionList = new List<string>(streamsToReadBeforeExecution.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                if (streamsToReadBeforeExecutionList.Contains(streamType))
                {
                    TraceManager.PipelineComponent.TraceInfo("{0} - Reading stream to ensure it's read logic get's executed prior to pipeline component execution", callToken);
                    StreamReader reader = new StreamReader(copiedBodyPart.Data);
                    pc.ResourceTracker.AddResource(reader);
                    copiedBodyPart.Data.Position = 0;
                }
                else
                {
                    TraceManager.PipelineComponent.TraceInfo("{0} - No need to read stream as stream type does not match entries in StreamsToReadBeforeExecution parameter", callToken);
                }
            }
            else
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - No need to read stream as there are no entries in StreamsToReadBeforeExecution parameter", callToken);
            }
        }

        /// <summary>
        /// Copy over all parts of the message from a source message to a destination message, choosing which part should be the body part
        /// </summary>
        private void CopyMessageParts(IBaseMessage sourceMessage, IBaseMessage destinationMessage, IBaseMessagePart newBodyPart)
        {
            string bodyPartName = sourceMessage.BodyPartName;
            for (int partCounter = 0; partCounter < sourceMessage.PartCount; ++partCounter)
            {
                string partName = null;
                IBaseMessagePart messagePart = sourceMessage.GetPartByIndex(partCounter, out partName);
                partNames.Add(partCounter, partName);

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
        /// Instantiate facts that will be used by the InstructionLoaderPolicy
        /// </summary>
        private void InstantiateInstructionLoaderPolicyFacts(Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            documentWrapper = new TypedXMLDocumentWrapper(_BREPipelineMetaInstructionCollection.InMsg.BodyPart.GetOriginalDataStream(), pc);
            sqlConnectionCollection = new SQLDataConnectionCollection();

            object property = _BREPipelineMetaInstructionCollection.InMsg.Context.Read("MessageType", ContextPropertyNamespaces._BTSPropertyNamespace);

            if (property != null)
            {
                utility = new MessageUtility(_BREPipelineMetaInstructionCollection.InMsg.BodyPart.GetOriginalDataStream(), property.ToString(), pc);
            }
            else
            {
                utility = new MessageUtility(_BREPipelineMetaInstructionCollection.InMsg.BodyPart.GetOriginalDataStream(), pc);
            }
        }

        /// <summary>
        /// Execute the policy in question, utilizing the DebutTrackingInspector if a TrackingFolder has been specified, and applying specific policy versions 
        /// if they have been specified
        /// </summary>
        private void ExecutePolicy(string policyName, object[] facts, string policyVersion)
        {
            DebugTrackingInterceptor dti = null;
            Microsoft.RuleEngine.Policy policy = null;

            if (string.IsNullOrEmpty(policyVersion))
            {
                policy = new Policy(policyName);
            }
            else
            {
                int majorVersion;
                int minorVersion;

                string[] versionArray = policyVersion.Split('.');

                if (versionArray.Length != 2)
                {
                    throw new Exception(string.Format("Version number {0} for policy {1} did not correctly resolve to a proper major and minor version number.", policyVersion, policyName));
                }

                if (!int.TryParse(versionArray[0], out majorVersion))
                {
                    throw new Exception(string.Format("Major version number {0} for policy {1} is invalid.", versionArray[0], policyName));
                }

                if (!int.TryParse(versionArray[1], out minorVersion))
                {
                    throw new Exception(string.Format("Minor version number {0} for policy {1} is invalid.", versionArray[1], policyName));
                }

                policy = new Policy(policyName, majorVersion, minorVersion);
            }

            try
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Executing Policy {1} {2}.{3}", callToken, policy.PolicyName, policy.MajorRevision.ToString(), policy.MinorRevision.ToString());

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

                    dti = new DebugTrackingInterceptor(trackingFolder + String.Format("{0}_{1}.{2}_{3}.txt",
                        policy.PolicyName, policy.MajorRevision.ToString(), policy.MinorRevision.ToString(), callToken));

                    policy.Execute(facts, dti);
                }
            }
            catch (PolicyExecutionException ex)
            {
                TraceManager.PipelineComponent.TraceError(ex, true, Guid.Parse(callToken));

                string exceptionMessage = string.Format("Exception encountered while executing BRE policy {0}.  Top level exception message - {1}", policyName, ex.InnerException.Message);

                if (ex.InnerException.InnerException != null)
                {
                    exceptionMessage = exceptionMessage + Environment.NewLine;
                    exceptionMessage = exceptionMessage + string.Format("Innermost exception was - {0}", ex.GetBaseException());
                }

                throw new Exception(exceptionMessage);
            }
            catch (Exception e)
            {
                TraceManager.PipelineComponent.TraceError(e, true, Guid.Parse(callToken));
                throw;
            }
            finally
            {
                if (dti != null)
                {
                    dti.CloseTraceFile();
                }

                if (policy != null)
                {
                    policy.Dispose();
                }
            }
        }

        /// <summary>
        /// Override the default ExecutionPolicy name and version, ApplicationContext, and XMLFactsApplicationStage if an override instruction was set by the InstructionLoaderPolicy
        /// </summary>
        private void ApplyOverrides()
        {
            // Override the Execution Policy if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (!String.IsNullOrEmpty(_BREPipelineMetaInstructionCollection.ExecutionPolicyOverride))
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Overriding the Execution Policy to {1}", callToken, _BREPipelineMetaInstructionCollection.ExecutionPolicyOverride);
                executionPolicy = _BREPipelineMetaInstructionCollection.ExecutionPolicyOverride;
            }

            // Override the Execution Policy version if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (!String.IsNullOrEmpty(_BREPipelineMetaInstructionCollection.ExecutionPolicyVersionOverride))
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Overriding the Execution Policy version to {1}", callToken, _BREPipelineMetaInstructionCollection.ExecutionPolicyVersionOverride);
                executionPolicyVersion = _BREPipelineMetaInstructionCollection.ExecutionPolicyVersionOverride;
            }

            // Override the Application Context if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (!String.IsNullOrEmpty(_BREPipelineMetaInstructionCollection.ApplicationContextOverride))
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Overriding the Application Context to {1}", callToken, _BREPipelineMetaInstructionCollection.ApplicationContextOverride);
                applicationContext = _BREPipelineMetaInstructionCollection.ApplicationContextOverride;
            }

            // Override the XML Facts application stage if an instruction to override it was received during execution of the InstructionLoaderPolicy
            if (_BREPipelineMetaInstructionCollection.XmlFactsApplicationStageOverride != _XMLFactsApplicationStage)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Overriding the XML Facts Application Stage to {1}", callToken, _BREPipelineMetaInstructionCollection.XmlFactsApplicationStageOverride);
                _XMLFactsApplicationStage = _BREPipelineMetaInstructionCollection.XmlFactsApplicationStageOverride;
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
                TraceManager.PipelineComponent.TraceInfo("{0} - Instantiating the message since it was null and the pipeline component needs to assign the TypedXMLDocument to it.", callToken);
                _BREPipelineMetaInstructionCollection.InMsg = pc.GetMessageFactory().CreateMessage();
                _BREPipelineMetaInstructionCollection.InMsg.Context = PipelineUtil.CloneMessageContext(inmsg.Context);
            }

            // If the message body part is null then instantiate it again so we can assign the typed xml document stream to it
            if (_BREPipelineMetaInstructionCollection.InMsg.BodyPart == null)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Instantiating the message body part since it was null and the pipeline component needs to assign the TypedXMLDocument to it.", callToken);
                IBaseMessagePart messageBodyPart = pc.GetMessageFactory().CreateMessagePart();
                _BREPipelineMetaInstructionCollection.InMsg.AddPart("Body", messageBodyPart, true);
            }
        }

        /// <summary>
        /// Add out of the box MetaInstructions to the collection so they can be used in any ExecutionPolicy
        /// </summary>
        private void AddOutOfTheBoxMetaInstructions()
        {
            // Add the out of the box MetaInstruction classes to the 
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(new ContextMetaInstructions());
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(new HelperMetaInstructions());
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(new CachingMetaInstructions());
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(new MessagePartMetaInstructions());
            _BREPipelineMetaInstructionCollection.AddMetaInstruction(new XMLTranslatorMetaInstructions());

            // Only add TypedXMLDocumentMetaInstructions if a TypedXMLDocument has been setup
            if (documentWrapper.DocumentCount == 1)
            {
                _BREPipelineMetaInstructionCollection.AddMetaInstruction(new TypedXMLDocumentMetaInstructions(documentWrapper.Document, _XMLFactsApplicationStage));
            }
        }

        /// <summary>
        /// Setup the ExecutionPolicy with the appropriate facts
        /// </summary>
        private object[] SetupExecutionPolicyFacts()
        {
            List<object> factsList = new List<object>();

            factsList.Add(applicationContext);
            factsList.Add(_BREPipelineMetaInstructionCollection.Pc);

            // Add the TypedXmlDocument to the fact list if it has been setup
            if (documentWrapper.DocumentCount == 1)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Adding TypedXMLDocument of type {1} to Execution Policy facts.", callToken, documentWrapper.Document.DocumentType);
                factsList.Add(documentWrapper.Document);
            }

            // Add any SQL DataConnection that were added to by the InstructionLoaderPolicy to the fact array
            for (int i = 1; i <= sqlConnectionCollection.SQLConnectionCount; i++)
            {
                string dbName;
                string dbTable;
                sqlConnectionCollection.GetDataConnectionDetailsByIndex(i - 1, out dbName, out dbTable);
                TraceManager.PipelineComponent.TraceInfo("{0} - Adding SQL Connection for database {1} and table {2} to Execution Policy facts.", callToken, dbName, dbTable);
                factsList.Add(sqlConnectionCollection.GetDataConnectionByIndex(i - 1));
            }

            // Add the out of the box MetaInstructions and any other added to the MetaInstructionCollection by the InstructionLoaderPolicy to the fact array
            for (int i = 1; i <= _BREPipelineMetaInstructionCollection.GetCount(); i++)
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Adding MetaInstruction {1} to Execution Policy facts.", callToken, _BREPipelineMetaInstructionCollection.GetMetaInstructionByIndex(i - 1).ToString());
                factsList.Add(_BREPipelineMetaInstructionCollection.GetMetaInstructionByIndex(i - 1));
            }

            return factsList.ToArray();
        }

        /// <summary>
        /// Set the message back to the original message, ensure it's body part is seekable, set the position back to 0, and apply RIP context properties
        /// </summary>
        private void HandleRIPException(Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Exception e)
        {
            TraceManager.PipelineComponent.TraceInfo("{0} - Handling error in a recoverable interchange processing fashion", callToken);

            _BREPipelineMetaInstructionCollection.InMsg = inmsg;
            _BREPipelineMetaInstructionCollection.InMsg.BodyPart.Data = originalStream;
            _BREPipelineMetaInstructionCollection.InMsg.BodyPart.Data.Position = 0;           
            _BREPipelineMetaInstructionCollection.InMsg.SetErrorInfo(e);
            _BREPipelineMetaInstructionCollection.InMsg.Context.Write(BizTalkGlobalPropertySchemaEnum.MessageDestination.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace,
                "SuspendQueue");
        }
        
        #endregion
    }
}
