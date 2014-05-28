using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.TestSampleInstructions
{
    public class MetaInstruction : BREPipelineMetaInstructionBase
    {
        public string GetBREPipelineFrameworkContextProperty(BPFEnum property, FailureActionEnum failureAction)
        {
            string propertyValue = "";

            try
            {
                propertyValue = base.InMsg.Context.Read(property.ToString(), "https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema").ToString();
            }
            catch (Exception e)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get BREPipelineFramework context property " + property + ".  Error encountered was - " + e.Message, e);
                    base.SetException(exc);
                }
            }

            return propertyValue;
        }

        public void SetBREPipelineFrameworkContextProperty(BPFEnum propertyName, object value, ContextInstructionTypeEnum promotion, TypeEnum type)
        {
            Instruction instruction = new Instruction(propertyName.ToString(), value, promotion, type);
            base.AddInstruction(instruction);
        }
    }
}
