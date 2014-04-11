using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.SSOClient.Interop;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class CreateSSOTicketInstruction : IBREPipelineInstruction
    {
        public CreateSSOTicketInstruction()
        {
        }

        public void Execute(ref IBaseMessage inmsg, IPipelineContext pc)
        {
            try
            {
                ISSOTicket ssoTicket = new ISSOTicket();
                inmsg.Context.Write("SSOTicket", "http://schemas.microsoft.com/BizTalk/2003/system-properties", ssoTicket.IssueTicket(0)); 
            }
            catch (Exception e)
            {
                throw new Exception("Unable to set context property http://schemas.microsoft.com/BizTalk/2003/system-properties#SSOTicket. Encountered error - " + e.ToString());
            }
        }
    }
}
