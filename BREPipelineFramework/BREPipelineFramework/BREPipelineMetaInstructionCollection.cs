using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using BREPipelineFramework.Helpers;
using System.Diagnostics;

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

        [ThreadStatic]
        public static int instructionCounter;

        #endregion

        #region Public Properties

        public IBaseMessage InMsg
        {
            get { return inMsg; }
            set { inMsg = value; }
        }

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

        #endregion

        #region Constructors

        public BREPipelineMetaInstructionCollection(XMLFactsApplicationStageEnum xmlFactsApplicationStageOverride, InstructionExecutionOrderEnum instructionExecutionOrder)
        {
            this.xmlFactsApplicationStageOverride = xmlFactsApplicationStageOverride;
            this.instructionExecutionOrder = instructionExecutionOrder;
        }

        #endregion

        #region Public Methods

        public static int GetLatestCounter()
        {
            BREPipelineMetaInstructionCollection.instructionCounter = BREPipelineMetaInstructionCollection.instructionCounter + 1;
            return BREPipelineMetaInstructionCollection.instructionCounter;
        }

        /// <summary>
        /// Instantiate and add a MetaInstruction to the MetaInstructionCollection by specifying the MetaInstruction's class name and assembly name
        /// </summary>
        /// <param name="MetaInstructionClass"></param>
        /// <param name="MetaInstructionAssembly"></param>
        public void AddMetaInstruction(string MetaInstructionClass, string MetaInstructionAssembly)
        {
            try
            {
                //Instantiate the MetaInstruction
                Type t = Type.GetType(MetaInstructionClass + "," + MetaInstructionAssembly);
                ConstructorInfo info = t.GetConstructor(Type.EmptyTypes);
                ObjectCreator inv = new ObjectCreator(info);
                object o = null;
                o = inv.CreateInstance();

                if (o != null)
                {
                    //If the instantiated object isn't null then add it to the MetaInstructionCollection
                    BREPipelineMetaInstructionBase metaInstruction = (BREPipelineMetaInstructionBase)o;
                    AddMetaInstruction(metaInstruction);
                }
                else
                {
                    //If the instantiated object is null then set _BREException which will be thrown by the pipeline component
                    _BREException = new Exception("Unable to instantiate MetaInstruction");
                }
            }
            catch (Exception e)
            {
                //Set any caught exceptions to _BREException which will be thrown the pipeline component
                _BREException = e;
            }
        }

        public void AddMetaInstruction(BREPipelineMetaInstructionBase metaInstruction)
        {
            string key = metaInstruction.GetType().ToString();

            if (!metaInstructionCollection.ContainsKey(key))
            {
                metaInstruction._InMsg = inMsg;
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
        /// Execute all the MetaInstructions in the MetaInstructionCollection
        /// </summary>
        public void Execute()
        {
            if (instructionExecutionOrder != InstructionExecutionOrderEnum.Legacy && instructionExecutionOrder != InstructionExecutionOrderEnum.RulesExecution)
            {
                throw new Exception("Unexpected InstructionExecutionOrder value of " + instructionExecutionOrder);
            }


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
                foreach (var metaInstruction in metaInstructionCollection)
                {
                    Dictionary<int, IBREPipelineInstruction> instructions = metaInstruction.Value.GetInstructionCollection();

                    foreach (var instruction in instructions)
                    {
                        orderedInstructionList.Add(instruction.Key, instruction.Value);
                    }
                }

                foreach (var instruction in orderedInstructionList)
                {
                    instruction.Value.Execute(ref inMsg, pc);
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
