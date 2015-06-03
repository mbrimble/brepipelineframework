using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.Helpers.Tracing;

namespace BREPipelineFramework
{
    public class BREPipelineMetaInstructionCollection
    {
        #region Private Properties

        private SortedDictionary<string, BREPipelineMetaInstructionBase> metaInstructionCollection = new SortedDictionary<string, BREPipelineMetaInstructionBase>();
        private SortedList<int, IBREPipelineInstruction> orderedInstructionList = new SortedList<int, IBREPipelineInstruction>();
        private IBaseMessage inMsg;
        private IPipelineContext pc;
        private Exception _BREException = null;
        private string executionPolicyOverride;
        private string applicationContextOverride;
        private XMLFactsApplicationStageEnum xmlFactsApplicationStageOverride;
        private InstructionExecutionOrderEnum instructionExecutionOrder;
        private string executionPolicyVersionOverride;
        private Dictionary<int, string> partNames = new Dictionary<int, string>();
        private string callToken;

        #endregion

        #region Public Properties

        /// <summary>
        /// The output message of the pipeline component
        /// </summary>
        public IBaseMessage InMsg
        {
            get { return inMsg; }
            set { inMsg = value; }
        }

        /// <summary>
        /// The pipeline context
        /// </summary>
        public IPipelineContext Pc
        {
            get { return pc; }
            set { pc = value; }
        }

        /// <summary>
        /// Provides for the ability to override the chosen ExecutionPolicy during execution of the InstructionLoaderPolicy
        /// </summary>
        public string ExecutionPolicyOverride
        {
            get { return executionPolicyOverride; }
            set { executionPolicyOverride = value; }
        }

        /// <summary>
        /// Provides for the ability to override the chosen ExecutionPolicy version during execution of the InstructionLoaderPolicy
        /// </summary>
        public string ExecutionPolicyVersionOverride
        {
            get { return executionPolicyVersionOverride; }
            set { executionPolicyVersionOverride = value; }
        }

        /// <summary>
        /// Provides for the ability to override the chosen Application Context during execution of the InstructionLoaderPolicy
        /// </summary>
        public string ApplicationContextOverride
        {
            get { return applicationContextOverride; }
            set { applicationContextOverride = value; }
        }

        /// <summary>
        /// Provides the ability to override the chosen XMLFactsApplictionStage during execution of the InstructionLoaderPolicy
        /// </summary>
        public XMLFactsApplicationStageEnum XmlFactsApplicationStageOverride
        {
            get { return xmlFactsApplicationStageOverride; }
            set { xmlFactsApplicationStageOverride = value; }
        }

        /// <summary>
        /// Exposes exceptions that were handled during execution of the InstructionLoaderPolicy
        /// </summary>
        public Exception BREException
        {
            get { return _BREException; }
        }

        #endregion

        #region Constructors

        public BREPipelineMetaInstructionCollection(XMLFactsApplicationStageEnum xmlFactsApplicationStageOverride, InstructionExecutionOrderEnum instructionExecutionOrder, Dictionary<int, string> partNames, string callToken)
        {
            this.xmlFactsApplicationStageOverride = xmlFactsApplicationStageOverride;
            this.instructionExecutionOrder = instructionExecutionOrder;
            this.partNames = partNames;
            this.callToken = callToken;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Instantiate and add a MetaInstruction to the MetaInstructionCollection by specifying the MetaInstruction's class name and assembly name
        /// </summary>
        /// <param name="MetaInstructionClass"></param>
        /// <param name="MetaInstructionAssembly"></param>
        public void AddMetaInstruction(string MetaInstructionClass, string MetaInstructionAssembly)
        {
            string fullyQualifiedClass = MetaInstructionClass + "," + MetaInstructionAssembly;
            AddMetaInstruction(fullyQualifiedClass);
        }

        public void AddMetaInstruction(string fullyQualifiedClass)
        {
            try
            {
                //Resolve the metaInstructionType based on the fully qualified class/assembly name, and then create an instance
                //of the metaInstruction
                Type metaInstructionType = ObjectCreator.ResolveType(fullyQualifiedClass);
                object o = ObjectCreator.CreateConstructorlessInstance(metaInstructionType);

                if (o != null)
                {
                    //If the instantiated object isn't null then add it to the MetaInstructionCollection
                    BREPipelineMetaInstructionBase metaInstruction = (BREPipelineMetaInstructionBase)o;
                    AddMetaInstruction(metaInstruction);
                }
                else
                {
                    //If the instantiated object is null then set _BREException which will be thrown by the pipeline component
                    _BREException = new Exception("Unable to instantiate MetaInstruction - " + fullyQualifiedClass);
                }
            }
            catch (Exception e)
            {
                //Set any caught exceptions to _BREException which will be thrown by the pipeline component
                _BREException = new Exception("Unable to instantiate MetaInstruction - " + fullyQualifiedClass
                    + ", exception encountered - " + e.Message);
            }
        }

        /// <summary>
        /// Add a given metaInstruction instance to the collection
        /// </summary>
        public void AddMetaInstruction(BREPipelineMetaInstructionBase metaInstruction)
        {
            string key = metaInstruction.GetType().ToString();

            if (instructionExecutionOrder == InstructionExecutionOrderEnum.RulesExecution)
            {
                metaInstruction.InstructionCollection = orderedInstructionList;
            }

            metaInstruction.PartNames = partNames;

            if (!metaInstructionCollection.ContainsKey(key))
            {
                metaInstruction._InMsg = inMsg;
                metaInstruction.Pc = pc;
                metaInstruction.CallToken = callToken;
                metaInstructionCollection.Add(key, metaInstruction);
            }
        }

        /// <summary>
        /// Get a count of MetaInstructions in the MetaInstructionCollection
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return metaInstructionCollection.Count();
        }

        /// <summary>
        /// Get a MetaInstruction from the MetaInstructionCollection by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BREPipelineMetaInstructionBase GetMetaInstructionByIndex(int index)
        {
            return metaInstructionCollection.ElementAt(index).Value;
        }

        /// <summary>
        /// Execute all the MetaInstructions in the MetaInstructionCollection respecting the order in which instructions were added
        /// or respecting pre v1.5 legacy behavior
        /// </summary>
        public void Execute()
        {
            TraceManager.PipelineComponent.TraceInfo("{0} - Starting to execute all MetaInstructions.", callToken);

            foreach (var metaInstruction in metaInstructionCollection)
            {
                metaInstruction.Value.ExecutionPreProcessing();

                if (instructionExecutionOrder == InstructionExecutionOrderEnum.Legacy)
                {
                    metaInstruction.Value.ExecuteAllBREPipelineInstructions(ref inMsg, pc);
                }
            }

            if (instructionExecutionOrder == InstructionExecutionOrderEnum.RulesExecution)
            {
                foreach (var instruction in orderedInstructionList)
                {
                    if (instruction.Value != null)
                    {
                        TraceManager.PipelineComponent.TraceInfo("{0} - Executing instruction {1}.", callToken, instruction.Value.ToString());
                        instruction.Value.Execute(ref inMsg, pc);
                    }
                }
            }

            foreach (var metaInstruction in metaInstructionCollection)
            {
                metaInstruction.Value.ExecutionPostProcessing();
            }
        }

        /// <summary>
        /// Compensate all the MetaInstructions in the MetaInstructionCollection
        /// </summary>
        public void Compensate()
        {
            TraceManager.PipelineComponent.TraceInfo("{0} - Performing compensation.", callToken);

            foreach (var metaInstruction in metaInstructionCollection)
            {
                metaInstruction.Value.Compensate();
            }
        }

        /// <summary>
        /// Throw any exceptions contained within MetaInstructions in the MetaInstructionCollection
        /// </summary>
        public void ThrowExceptions()
        {
            foreach (var metaInstruction in metaInstructionCollection)
            {
                if (metaInstruction.Value.BREException != null)
                {
                    Compensate();
                    throw metaInstruction.Value.BREException;
                }
            }
        }

        #endregion
    }
}
