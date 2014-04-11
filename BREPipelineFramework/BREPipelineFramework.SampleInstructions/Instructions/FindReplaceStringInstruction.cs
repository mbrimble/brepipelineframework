//Code borrowed from http://code.msdn.microsoft.com/windowsdesktop/Build-BizTalk-Custom-570dc663

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.BizTalk.Streaming;

//Note typo in namespace however changing this will result in a breaking change so leave as it is
namespace BREPipelineFramework.SampeInstructions.Instructions
{
    public class FindReplaceStringInstruction : IBREPipelineInstruction
    {
        private Dictionary<string, string> stringReplaceCollection;
        private Dictionary<string, string> regexReplaceCollection;

        public FindReplaceStringInstruction()
        {
            stringReplaceCollection = new Dictionary<string, string>();
            regexReplaceCollection = new Dictionary<string, string>();
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            VirtualStream newMsgStream = new VirtualStream();
            inmsg.BodyPart.Data.CopyTo(newMsgStream);
            newMsgStream.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader reader = new System.IO.StreamReader(newMsgStream);
            string body = reader.ReadToEnd();
            reader.Close();

            foreach (KeyValuePair<string, string> kp in stringReplaceCollection)
            {
                body = body.Replace(kp.Key, kp.Value);
            }

            foreach (KeyValuePair<string, string> kp in regexReplaceCollection)
            {
                body = Regex.Replace(body, kp.Key, kp.Value);
            }

            System.IO.MemoryStream updatedStream = new System.IO.MemoryStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(updatedStream);
            writer.AutoFlush = true;
            writer.Write(body);
            updatedStream.Position = 0;
            inmsg.BodyPart.Data = updatedStream;
        }

        internal void AddStringReplace(string stringToReplace, string stringToReplaceWith)
        {
            stringReplaceCollection.Add(stringToReplace, stringToReplaceWith);
        }

        internal void AddRegexReplace(string regexToReplace, string stringToReplaceWith)
        {
            regexReplaceCollection.Add(regexToReplace, stringToReplaceWith);
        }
    }
}
