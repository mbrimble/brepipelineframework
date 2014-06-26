using System;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.SampleInstructions.Instructions;
using Microsoft.BizTalk.Message.Interop;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class MessagePartMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Public Methods

        public void SetCustomMessagePartProperty(string propertyName, string propertyNamespace, MessagePartIdentifierType identifierType, string identifier, object value, TypeEnum type)
        {
            identifier = GetPartName(identifierType, identifier);
            SetMessagePartPropertyPipelineInstruction instruction = new SetMessagePartPropertyPipelineInstruction(propertyName, propertyNamespace, identifier, value, type);
            base.AddInstruction(instruction);
        }

        public void SetCustomMessageBodyPartProperty(string propertyName, string propertyNamespace, object value, TypeEnum type)
        {
            string identifier = InMsg.BodyPartName;
            SetMessagePartPropertyPipelineInstruction instruction = new SetMessagePartPropertyPipelineInstruction(propertyName, propertyNamespace, identifier, value, type);
            base.AddInstruction(instruction);
        }

        public void SetMIMEFileNameMessagePartProperty(MessagePartIdentifierType identifierType, string identifier, string value)
        {
            SetCustomMessagePartProperty(BizTalkMIMEPropertySchemaEnum.FileName.ToString(), ContextPropertyNamespaces._MIMEPropertyNamespace, identifierType, identifier, value, TypeEnum.String);
        }

        public void SetMIMEFileNameMessageBodyPartProperty(string value)
        {
            string identifier = InMsg.BodyPartName;
            SetMessagePartPropertyPipelineInstruction instruction = new SetMessagePartPropertyPipelineInstruction(BizTalkMIMEPropertySchemaEnum.FileName.ToString(), ContextPropertyNamespaces._MIMEPropertyNamespace,
                identifier, value);
            base.AddInstruction(instruction);
        }

        public string GetCustomMessagePartProperty(string propertyName, string propertyNamespace, MessagePartIdentifierType identifierType, string identifier, FailureActionEnum failureAction)
        {
            identifier = GetPartName(identifierType, identifier);

            object property = base.InMsg.GetPart(identifier).PartProperties.Read(propertyName, propertyNamespace);
            string propertyValue = null;

            if (property != null)
            {
                propertyValue = property.ToString();
            }
            else
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to get part property " + propertyName + " in namespace " + propertyNamespace + " on part named " + identifier + ".");
                    base.SetException(exc);
                }
                else if (failureAction == FailureActionEnum.DefaultForType)
                {
                    propertyValue = string.Empty;
                }
                else if (failureAction == FailureActionEnum.Null)
                {
                    // Do nothing, leave as null
                }
            }

            return propertyValue;
        }

        public string GetCustomMessageBodyPartProperty(string propertyName, string propertyNamespace, FailureActionEnum failureAction)
        {
            return GetCustomMessagePartProperty(propertyName, propertyNamespace, MessagePartIdentifierType.ByPartName, InMsg.BodyPartName, failureAction);
        }

        public string GetMessagePartNameAtIndex(int index)
        {
            return GetPartName(MessagePartIdentifierType.ByPartIndex, index.ToString());
        }

        public string GetMessageBodyPartName()
        {
            return InMsg.BodyPartName;
        }

        public string GetMessagePartContentType(MessagePartIdentifierType identifierType, string identifier)
        {
            identifier = GetPartName(identifierType, identifier);
            return InMsg.GetPart(identifier).ContentType;
        }

        public string GetMessageBodyPartContentType()
        {
            return InMsg.BodyPart.ContentType;
        }

        public string GetMessagePartCharSet(MessagePartIdentifierType identifierType, string identifier)
        {
            identifier = GetPartName(identifierType, identifier);
            return InMsg.GetPart(identifier).Charset;
        }

        public string GetMessageBodyPartCharSet()
        {
            return InMsg.BodyPart.Charset;
        }

        public void SetMessagePartContentType(MessagePartIdentifierType identifierType, string identifier, string contentType)
        {
            identifier = GetPartName(identifierType, identifier);
            SetMessagePartDetailsPipelineInstruction instruction = new SetMessagePartDetailsPipelineInstruction(contentType, PartDetailTypeEnum.ContentType, identifier);
            base.AddInstruction(instruction);            
        }

        public void SetMessageBodyPartContentType(string contentType)
        {
            SetMessagePartDetailsPipelineInstruction instruction = new SetMessagePartDetailsPipelineInstruction(contentType, PartDetailTypeEnum.ContentType, InMsg.BodyPartName);
            base.AddInstruction(instruction);                        
        }

        public void SetMessagePartCharSet(MessagePartIdentifierType identifierType, string identifier, string charSet)
        {
            identifier = GetPartName(identifierType, identifier);
            SetMessagePartDetailsPipelineInstruction instruction = new SetMessagePartDetailsPipelineInstruction(charSet, PartDetailTypeEnum.CharSet, identifier);
            base.AddInstruction(instruction);
        }

        public void SetMessageBodyPartCharSet(string charSet)
        {
            SetMessagePartDetailsPipelineInstruction instruction = new SetMessagePartDetailsPipelineInstruction(charSet, PartDetailTypeEnum.CharSet, InMsg.BodyPartName);
            base.AddInstruction(instruction);            
        }

        public int GetPartCount()
        {
            return InMsg.PartCount;
        }

        #endregion

        #region Private methods

        private string GetPartName(MessagePartIdentifierType identifierType, string identifier)
        {
            if (identifierType == MessagePartIdentifierType.ByPartIndex)
            {
                int partId;
                bool validPartId = int.TryParse(identifier, out partId);

                if (!validPartId)
                {
                    base.SetException(new Exception("Invalid part index of " + identifier));
                }

                if (InMsg.PartCount <= partId)
                {
                    base.SetException(new Exception(string.Format("Part index {0} is greater than the part count", identifier)));
                }

                identifier = PartNames[partId];

                InMsg.GetPartByIndex(partId, out identifier);
            }
            else if (identifierType == MessagePartIdentifierType.ByPartName)
            {
                //Do Nothing
            }
            else
            {
                base.SetException(new Exception("Invalid MesssagePartIdentifierType of " + identifierType.ToString()));
            }
            return identifier;
        }

        #endregion
    }
}
