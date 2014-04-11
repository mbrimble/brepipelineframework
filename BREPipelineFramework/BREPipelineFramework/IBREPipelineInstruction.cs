using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework
{
    public interface IBREPipelineInstruction
    {
        /// <summary>
        /// Execute the logic of the BRE Pipeline Instruction
        /// </summary>
        /// <param name="inmsg"></param>
        /// <param name="pc"></param>
        void Execute(ref IBaseMessage inmsg, IPipelineContext pc);
    }
}
