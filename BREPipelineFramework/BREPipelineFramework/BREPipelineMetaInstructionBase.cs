using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework
{
    public abstract class BREPipelineMetaInstructionBase
    {
        #region Private/Internal properties

        private List<IBREPipelineInstruction> instructionCollection;
        private Exception _BREException = null;
        private IBaseMessage inMsg;

        internal IBaseMessage _InMsg
        {
            get { return inMsg; }
            set { inMsg = value; }
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
        /// BREException will contain the last exception that was encountered by an Instruction that was contained within the MetaInstruction's collection of Instructions
        /// </summary>
        internal Exception BREException
        {
            get { return _BREException; }
        }

        #endregion

        #region Constructors

        public BREPipelineMetaInstructionBase()
        {
            //Instantiate the instructionCollection so that Instructions can be added to it
            this.instructionCollection = new List<IBREPipelineInstruction>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add an Instruction to the collection of Instructions contained within this MetaInstruction
        /// </summary>
        /// <param name="instruction">Instruction to add to the collection</param>
        public void AddInstruction(IBREPipelineInstruction instruction)
        {
            instructionCollection.Add(instruction);
        }

        /// <summary>
        /// Set an exception to the MetaInstruction rather than throwing it since thrown exceptions within policy execution aren't displayed well, the exception will
        /// will be thrown by the Pipeline Component
        /// </summary>
        /// <param name="exception"></param>
        public void SetException(Exception exception)
        {
            _BREException = exception;
        }

        /// <summary>
        /// Execute all Instructions contained within the MetaInstruction's collection of Instructions
        /// </summary>
        /// <param name="inmsg">The message to be manipulated by the </param>
        /// <param name="pc">The pipeline context</param>
        public void ExecuteBREPipelineMetaInstruction(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            foreach (var instruction in instructionCollection)
            {
                instruction.Execute(ref inmsg, pc);
            }
        }

        #endregion
    }
}
