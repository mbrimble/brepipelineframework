using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class SetContextPropertyPipelineInstruction : IBREPipelineInstruction
    {
        private string propertyName;
        private string propertyNamespace;
        private ContextInstructionTypeEnum promotion;
        private object value;
        private TypeEnum type;
        private bool castRequired = false;

        public SetContextPropertyPipelineInstruction(string propertyName, string propertyNamespace, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
            this.promotion = promotion;
            this.value = value;
            this.castRequired = true;
        }

        public SetContextPropertyPipelineInstruction(string propertyName, string propertyNamespace, object value, ContextInstructionTypeEnum promotion)
        {
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
            this.promotion = promotion;
            this.value = value;
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            try
            {
                if (castRequired)
                {
                    if (promotion == ContextInstructionTypeEnum.Write)
                    {
                        inmsg.Context.Write(propertyName, propertyNamespace, TypeCaster.GetTypedObject(value, type));
                    }
                    else if (promotion == ContextInstructionTypeEnum.Promote)
                    {
                        inmsg.Context.Promote(propertyName, propertyNamespace, TypeCaster.GetTypedObject(value, type));
                    }
                }
                else
                {
                    if (promotion == ContextInstructionTypeEnum.Write)
                    {
                        inmsg.Context.Write(propertyName, propertyNamespace, value);
                    }
                    else if (promotion == ContextInstructionTypeEnum.Promote)
                    {
                        inmsg.Context.Promote(propertyName, propertyNamespace, value);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to set context property " + propertyNamespace + "#" + propertyName + ". Encountered error - " + e.ToString());
            }
        }
    }
}
