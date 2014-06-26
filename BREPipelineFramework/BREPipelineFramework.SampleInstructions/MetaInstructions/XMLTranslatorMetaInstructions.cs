using System;
using Microsoft.BizTalk.Message.Interop;
using BREPipelineFramework.Helpers;
using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;
using Microsoft.BizTalk.XPath;
using System.Data.SqlClient;
using System.Data;
using BREPipelineFramework.SampeInstructions.Instructions;
using System.Text.RegularExpressions;
using BREPipelineFramework.SampleInstructions.Instructions;
using BREPipelineFramework.Helpers.Tracing;
using System.Globalization;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class XMLTranslatorMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private Properties

        private MessageModificationInstructions messageModificationInstructions;
        private int instructionIndex = -1;

        #endregion

        #region Public Helper Methods
        
        /// <summary>
        /// Add a namespace to the document root node
        /// </summary>
        public void AddRootNodeNamespace(string Namespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.AddRootNodeNamespaceAndPrefix, Namespace, prefix: "ns0");
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        /// <summary>
        /// Add a namespace and namespace prefix to the document root node
        /// </summary>
        public void AddRootNodeNamespaceAndPrefix(string Namespace, string NamespacePrefix)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.AddRootNodeNamespaceAndPrefix, Namespace, prefix:NamespacePrefix);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        /// <summary>
        /// Replace the namespace on the document root node
        /// </summary>
        public void ReplaceRootNodeNamespace(string Namespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateRootNodeNamespaceAndPrefix, Namespace, prefix: "ns0");
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        /// <summary>
        /// Replace the namespace and namespace prefix on the document root node
        /// </summary>
        public void ReplaceRootNodeNamespaceAndPrefix(string Namespace, string NamespacePrefix)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateRootNodeNamespaceAndPrefix, Namespace, prefix: NamespacePrefix);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void ReplaceNamespace(string OldNamespace, string NewNamespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateNamespaceAndPrefix, NewNamespace, OldNamespace);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void ReplaceNamespaceAndPrefix(string OldNamespace, string NewNamespace, string NewPrefix)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateNamespaceAndPrefix, NewNamespace, OldNamespace, NewPrefix);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void ReplacePrefixForGivenNamespace(string OldNamespace, string NewPrefix)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateNamespaceAndPrefix, oldNamespace: OldNamespace, prefix: NewPrefix);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void RemoveNamespace(string Namespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.RemoveNamespace, Namespace);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementValueByNodeName(string nodeName, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementValue, name:nodeName, value:value);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementValueByNodeNameAndNamespace(string nodeName, string _namespace, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementValue, name: nodeName, _namespace:_namespace,  
                value: value);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementValueByNodeNameAndOldValue(string nodeName, string oldValue, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementValue, name: nodeName,
                oldValue: oldValue, value: value);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementValueByNodeNameNamespaceAndOldValue(string nodeName, string _namespace, string oldValue, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementValue, name: nodeName, _namespace: _namespace,
                oldValue: oldValue, value: value);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementValueByOldValue(string oldValue, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementValue,
                oldValue: oldValue, value: value);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementNameByOldName(string oldName, string name)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementName, name: name, oldName: oldName);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateElementNameByOldNameAndNamespace(string oldName, string oldNamespace, string name)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateElementName, name: name, oldName: oldName, oldNamespace: oldNamespace);
            AddMessageModificationInstruction(messageModificationInstruction);
        }
        
        public void UpdateAttributeValueByName(string localName, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateAttributeValue, oldName: localName, value: value);
            AddMessageModificationInstruction(messageModificationInstruction);            
        }

        public void UpdateAttributeValueByNameAndOldValue(string oldValue, string oldName, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateAttributeValue, oldValue: oldValue,
                oldName: oldName, value: value);
            AddMessageModificationInstruction(messageModificationInstruction);   
        }

        public void UpdateAttributeValueByNameAndNamespace(string localName, string _namespace, string value)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateAttributeValue, oldName: localName,
                oldNamespace: _namespace, value: value);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateAttributeNameByName(string oldName, string name)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateAttributeName, oldName: oldName, name: name);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void UpdateAttributeNameByNameAndNamespace(string oldName, string name, string oldNamespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateAttributeName, oldName: oldName,
                oldNamespace: oldNamespace, name: name);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void RemoveElementByName(string name)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.RemoveElement, name: name);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void RemoveElementByNameAndNamespace(string _namespace, string name)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.RemoveElement, name: name, _namespace:_namespace);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void RemoveAttributeByName(string name)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.RemoveAttribute, name: name);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void RemoveAttributeByNameAndNamespace(string name, string _namespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.RemoveAttribute, name: name, _namespace: _namespace);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        public void SetMessageModificationInstructionPriority()
        {
            if (instructionIndex == -1)
            {
                messageModificationInstructions = new MessageModificationInstructions(CallToken);
                instructionIndex = base.InstructionCount();
                base.AddInstruction(messageModificationInstructions);
            }
            else
            {
                base.ResetInstructionPriorityToEndOfQueue(instructionIndex);
                instructionIndex = base.InstructionCount();
            }
        }

        #endregion

        #region Private methods

        private void AddMessageModificationInstruction(MessageModificationDetails messageModificationInstruction)
        {
            if (messageModificationInstructions == null)
            {
                messageModificationInstructions = new MessageModificationInstructions(CallToken);
                instructionIndex = base.InstructionCount();
                base.AddInstruction(messageModificationInstructions);
            }

            messageModificationInstructions._MessageModificationDetails.Add(messageModificationInstruction);
        }

        #endregion
    }
}
