using System;
using System.IO;
using BREPipelineFramework.Helpers;
using System.Collections.Generic;

//Note typo in namespace however changing this will result in a breaking change so leave as it is
namespace BREPipelineFramework.SampeInstructions.Instructions
{
    public class MessageModificationInstructions : IBREPipelineInstruction
    {
        private List<MessageModificationDetails> _messageModificationDetails;
        private string callToken;

        public List<MessageModificationDetails> _MessageModificationDetails
        {
            get { return _messageModificationDetails; }
        }

        public MessageModificationInstructions(string callToken)
        {
            this._messageModificationDetails = new List<MessageModificationDetails>();
            this.callToken = callToken;
        }

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            var stream = new MessageModificationTranslatorStream(inmsg.BodyPart.GetOriginalDataStream(), _messageModificationDetails, callToken);
            inmsg.BodyPart.Data = stream;
            pc.ResourceTracker.AddResource(stream);
        }
    }
}
