using System.Collections.Generic;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.BizTalk.Streaming;
using BREPipelineFramework.Helpers.Tracing;

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
            System.IO.StreamReader reader = new System.IO.StreamReader(inmsg.BodyPart.GetOriginalDataStream());
            VirtualStream updatedStream = new VirtualStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(updatedStream, reader.CurrentEncoding);
            writer.AutoFlush = true;

            pc.ResourceTracker.AddResource(reader);
            pc.ResourceTracker.AddResource(updatedStream);
            pc.ResourceTracker.AddResource(writer);

            while (!reader.EndOfStream)
            {
                string body = reader.ReadLine();
                
                foreach (KeyValuePair<string, string> kp in stringReplaceCollection)
                {
                    body = body.Replace(kp.Key, kp.Value);
                }

                foreach (KeyValuePair<string, string> kp in regexReplaceCollection)
                {
                    body = Regex.Replace(body, kp.Key, kp.Value);
                }

                if (reader.EndOfStream)
                {
                    writer.Write(body);
                }
                else
                {
                    writer.WriteLine(body);
                }
            }

            updatedStream.Position = 0;
            inmsg.BodyPart.Data = updatedStream;
        }

        internal void AddStringReplace(string stringToReplace, string stringToReplaceWith)
        {
            stringReplaceCollection[stringToReplace] = stringToReplaceWith;
        }

        internal void AddRegexReplace(string regexToReplace, string stringToReplaceWith)
        {
            regexReplaceCollection[regexToReplace] = stringToReplaceWith;
        }
    }
}
