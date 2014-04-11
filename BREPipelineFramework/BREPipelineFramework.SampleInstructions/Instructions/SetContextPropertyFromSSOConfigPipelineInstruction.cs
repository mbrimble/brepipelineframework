using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.SSOClient.Interop;
using System.Collections.Specialized;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using BREPipelineFramework.Helpers;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class SetContextPropertyFromSSOConfigPipelineInstruction : IBREPipelineInstruction
    {
        private string propertyName;
        private string propertyNamespace;
        private ContextInstructionTypeEnum promotion;
        private string _SSOApplication;
        private string _SSOKey;
        private TypeEnum type;

        public SetContextPropertyFromSSOConfigPipelineInstruction(string propertyName, string propertyNamespace, ContextInstructionTypeEnum promotion, string _SSOApplication, string _SSOKey, TypeEnum type)
        {
            this.propertyName = propertyName;
            this.propertyNamespace = propertyNamespace;
            this.promotion = promotion;
            this._SSOApplication = _SSOApplication;
            this._SSOKey = _SSOKey;
            this.type = type;
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            try
            {
                object value = null;
                value = StaticHelpers.ReadFromSSO(_SSOApplication, _SSOKey);

                if (promotion == ContextInstructionTypeEnum.Write)
                {
                    inmsg.Context.Write(propertyName, propertyNamespace, TypeCaster.GetTypedObject(value, type));
                }
                else if (promotion == ContextInstructionTypeEnum.Promote)
                {
                    inmsg.Context.Promote(propertyName, propertyNamespace, TypeCaster.GetTypedObject(value, type));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to set context property " + propertyNamespace + "#" + propertyName + " from SSO application " + _SSOApplication + " and SSO Key " + _SSOKey + ". Encountered error - " + e.ToString());
            }
        }
    }
}
