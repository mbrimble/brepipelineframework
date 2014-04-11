using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/ms961978.aspx for an explanation of the SMTP context properties
    /// </summary>
    public enum BizTalkSMTPPropertySchemaEnum
    {
        Attachments = 0,
        CC = 1,
        DeliveryReceipt = 2,
        EmailBodyFile = 3,
        EmailBodyFileCharset = 4,
        EmailBodyText = 5,
        EmailBodyTextCharset = 6,
        From = 7,
        MessagePartsAttachments = 8,
        Password = 9,
        ReadReceipt = 10,
        ReplyBy = 11,
        SMTPAuthenticate = 12,
        SMTPHost = 13,
        Subject = 14,
        Username = 15
    }
}
