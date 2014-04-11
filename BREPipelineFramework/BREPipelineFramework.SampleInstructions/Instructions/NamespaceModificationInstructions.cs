using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;
using BREPipelineFramework.Helpers;

//Note typo in namespace however changing this will result in a breaking change so leave as it is
namespace BREPipelineFramework.SampeInstructions.Instructions
{
    class NamespaceModificationInstructions : IBREPipelineInstruction
    {
        private string _namespace;
        private string _namespacePrefix;
        private Boolean _replaceNamespace = false;

        public NamespaceModificationInstructions(string Namespace)
        {
            this._namespace = Namespace;
        }

        public NamespaceModificationInstructions(string Namespace, string NamespacePrefix)
        {
            this._namespace = Namespace;
            this._namespacePrefix = NamespacePrefix;
        }

        public NamespaceModificationInstructions(string Namespace, Boolean _replaceNamespace)
        {
            this._namespace = Namespace;
            this._replaceNamespace = _replaceNamespace;
        }

        public NamespaceModificationInstructions(string Namespace, string NamespacePrefix, Boolean _replaceNamespace)
        {
            this._namespace = Namespace;
            this._namespacePrefix = NamespacePrefix;
            this._replaceNamespace = _replaceNamespace;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            if (String.IsNullOrEmpty(_namespacePrefix))
            {
                var stream = new AddXmlNamespaceStream(inmsg.BodyPart.GetOriginalDataStream(), _namespace, _replaceNamespace);
                inmsg.BodyPart.Data = stream;
                pc.ResourceTracker.AddResource(stream);
            }
            else
            {
                var stream = new AddXmlNamespaceStream(inmsg.BodyPart.GetOriginalDataStream(), _namespace, _namespacePrefix, _replaceNamespace);
                inmsg.BodyPart.Data = stream;
                pc.ResourceTracker.AddResource(stream);
            }
        }
    }
}
