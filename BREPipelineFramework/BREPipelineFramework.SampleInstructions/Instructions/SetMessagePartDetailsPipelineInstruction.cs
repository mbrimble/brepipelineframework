using System;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class SetMessagePartPropertyPipelineInstruction : IBREPipelineInstruction
    {
        private string propertyName;
        private string propertyNamespace;
        private object value;
        private TypeEnum type;
        private bool castRequired = false;
        private string partName;

        public SetMessagePartPropertyPipelineInstruction(string propertyName, string propertyNamespace, string partName, object value, TypeEnum type)
        {
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
            this.value = value;
            this.castRequired = true;
            this.partName = partName;
            this.type = type;
        }

        public SetMessagePartPropertyPipelineInstruction(string propertyName, string propertyNamespace, string partName, object value)
        {
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
            this.value = value;
            this.partName = partName;
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            try
            {
                if (castRequired)
                {
                    inmsg.GetPart(partName).PartProperties.Write(propertyName, propertyNamespace, TypeCaster.GetTypedObject(value, type));
                }
                else
                {
                    inmsg.GetPart(partName).PartProperties.Write(propertyName, propertyNamespace, value);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to set part property " + propertyNamespace + "#" + propertyName + " on message part " + partName
                    + ". Encountered error - " + e.ToString());
            }
        }
    }
}
