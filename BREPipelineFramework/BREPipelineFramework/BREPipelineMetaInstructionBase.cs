using System;
using System.Collections.Generic;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using BREPipelineFramework.Helpers.Tracing;

namespace BREPipelineFramework
{
    public abstract class BREPipelineMetaInstructionBase
    {
        #region Private/Internal properties

        private SortedList<int, IBREPipelineInstruction> instructionCollection;
        private Exception _BREException = null;
        private IBaseMessage inMsg;
        private IPipelineContext pc;
        private Dictionary<int, string> partNames = new Dictionary<int, string>();
        private string callToken;

        internal IBaseMessage _InMsg
        {
            set { inMsg = value; }
        }

        /// <summary>
        /// BREException will contain the last exception that was encountered by an Instruction that was contained within the MetaInstruction's collection of Instructions
        /// </summary>
        internal Exception BREException
        {
            get { return _BREException; }
        }

        /// <summary>
        /// A collection of instructions that should be executed
        /// </summary>
        internal SortedList<int, IBREPipelineInstruction> InstructionCollection
        {
            set { instructionCollection = value; }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The BizTalk message that is to be manipulated, read only in MetaInstructions so it can be used for condition evaluation, it can only be manipulated in instructions
        /// </summary>
        public IBaseMessage InMsg
        {
            get { return inMsg; }
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
        /// A dictionary containing all the part names on the message by index
        /// </summary>
        public Dictionary<int, string> PartNames
        {
            get { return partNames; }
            set { partNames = value; }
        }

        /// <summary>
        /// A string representation of a tracking GUID to correlate trace statements for a given pipeline component instance
        /// </summary>
        public string CallToken
        {
            get { return callToken; }
            set { callToken = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add an Instruction to the collection of Instructions contained within this MetaInstruction
        /// </summary>
        /// <param name="instruction">Instruction to add to the collection</param>
        public virtual void AddInstruction(IBREPipelineInstruction instruction)
        {
            if (instructionCollection == null)
            {
                instructionCollection = new SortedList<int, IBREPipelineInstruction>();
            }

            TraceManager.PipelineComponent.TraceInfo("{0} - Adding Instruction {1} to the Instruction collection with a key of {2}.", callToken, instruction.ToString(), instructionCollection.Count);
            instructionCollection.Add(instructionCollection.Count, instruction);
        }

        public virtual void ResetInstructionPriorityToEndOfQueue(int oldKey)
        {
            if (instructionCollection == null)
            {
                return;
            }

            if (!instructionCollection.ContainsKey(oldKey))
            {
                return;
            }

            IBREPipelineInstruction instruction = instructionCollection[oldKey];

            TraceManager.PipelineComponent.TraceInfo("{0} - Setting instruction collection entry {1} which currently contains a {2} to null.", callToken, oldKey, instruction.ToString());
            instructionCollection[oldKey] = null;
            AddInstruction(instruction);
        }

        /// <summary>
        /// Set an exception to the MetaInstruction rather than throwing it since thrown exceptions within policy execution aren't displayed well
        /// (no longer true in v1.5 but still available if you want to explicitly thrown an exception) the exception will be thrown by the Pipeline Component
        /// </summary>
        public void SetException(Exception exception)
        {
            _BREException = exception;
        }

        /// <summary>
        /// Execute all Instructions contained within the MetaInstruction's collection of Instructions
        /// </summary>
        public void ExecuteAllBREPipelineInstructions(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            if (instructionCollection != null)
            {
                foreach (var instruction in instructionCollection)
                {
                    if (instruction.Value != null)
                    {
                        TraceManager.PipelineComponent.TraceInfo("{0} - Executing instruction {1} which has an order of {2} in the collection.", callToken, instruction.Value.ToString(), instruction.Key.ToString());
                        instruction.Value.Execute(ref inmsg, pc);
                    }
                }
            }
        }

        /// <summary>
        /// Perform pre processing prior to executing instructions
        /// </summary>
        public virtual void ExecutionPreProcessing()
        {
            // No base implementation but derived classes are free to implement this method
            // which will be called before instructions execute
        }

        /// <summary>
        /// Perform post processing after executing instructions
        /// </summary>
        public virtual void ExecutionPostProcessing()
        {
            // No base implementation but derived classes are free to implement this method
            // which will be called after instructions execute
        }

        /// <summary>
        /// Perform compensation in case any of the instructions being executed returned an exception
        /// </summary>
        public virtual void Compensate()
        {
            // No base implementation but derived classes are free to implement this method
            // which will be called in cases of exceptions executing MetaInstructions
        }

        /// <summary>
        /// Gets the count of instructions in the collection
        /// </summary>
        public int InstructionCount()
        {
            if (instructionCollection == null)
            {
                return 0;
            }
            else
            {
                return instructionCollection.Count;
            }
        }

        #endregion
    }
}
