using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.TestSampleInstructions
{
    public class Instruction : IBREPipelineInstruction
    {
        private string propertyName;
        private ContextInstructionTypeEnum promotion;
        private object value;
        private TypeEnum type;
        private bool throwException = false;

        public Instruction(string propertyName, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            this.propertyName = propertyName;
            this.promotion = promotion;
            this.value = value;
        }

        public Instruction()
        {
            throwException = true;
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            if (throwException)
            {
                throw new Exception("Will fail");
            }
            else
            {
                try
                {
                    if (promotion == ContextInstructionTypeEnum.Write)
                    {
                        inmsg.Context.Write(propertyName, "https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema", TypeCaster.GetTypedObject(value, type));
                    }
                    else if (promotion == ContextInstructionTypeEnum.Promote)
                    {
                        inmsg.Context.Promote(propertyName, "https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema", TypeCaster.GetTypedObject(value, type));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to set context property " + "https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema" + "#" + propertyName + ". Encountered error - " + e.ToString());
                }
            }
        }
    }
}
