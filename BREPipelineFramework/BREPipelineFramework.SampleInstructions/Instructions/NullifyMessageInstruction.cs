using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;

//Note typo in namespace however changing this will result in a breaking change so leave as it is
namespace BREPipelineFramework.SampeInstructions.Instructions
{
    public class NullifyMessageInstruction : IBREPipelineInstruction
    {
        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            inmsg = null;
        }
    }
}
