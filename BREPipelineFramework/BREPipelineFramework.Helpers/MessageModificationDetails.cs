using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.Helpers
{
    public class MessageModificationDetails
    {
        private string _namespace;
        private string oldNamespace;
        private string prefix;
        private string oldPrefix;
        private string value;
        private string oldValue;
        private string name;
        private string oldName;
        private MessageModificationInstructionTypeEnum modificationInstructionType;
        
        public string Namespace
        {
            get { return _namespace; }
        }

        public string OldNamespace
        {
            get { return oldNamespace; }
        }

        public string Prefix
        {
            get { return prefix; }
        }

        public string OldPrefix
        {
            get { return oldPrefix; }
        }

        public string Value
        {
            get { return this.value; }
        }

        public string OldValue
        {
            get { return oldValue; }
        }

        public string Name
        {
            get { return name; }
        }

        public string OldName
        {
            get { return oldName; }
        }

        public MessageModificationInstructionTypeEnum ModificationInstruction
        {
            get { return modificationInstructionType; }
        }

        public MessageModificationDetails(MessageModificationInstructionTypeEnum modificationInstruction, string _namespace = null, string oldNamespace = null, string prefix = null, 
            string oldPrefix = null, string value = null, string oldValue = null, string name = null, string oldName = null)
        {
            this.modificationInstructionType = modificationInstruction;
            this._namespace = _namespace;
            this.oldNamespace = oldNamespace;
            this.prefix = prefix;
            this.oldPrefix = oldPrefix;
            this.value = value;
            this.oldValue = oldValue;
            this.name = name;
            this.oldName = oldName;
        }
    }
}
