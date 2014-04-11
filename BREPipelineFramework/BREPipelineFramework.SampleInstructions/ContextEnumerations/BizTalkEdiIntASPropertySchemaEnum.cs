using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/bb226483(v=bts.10).aspx for an explanation of the AS2 context properties
    /// </summary>
    public enum BizTalkEdiIntASPropertySchemaEnum
    {
        AS2From = 0,
        AS2PayloadContentType = 1,
        AS2To = 2,
        DispositionMode = 3,
        DispositionType = 4,
        IsAS2AsynchronousMdn = 5,
        IsAS2FailedMessage = 6,
        IsAS2Http200OKResponse = 7,
        IsAS2MdnResponseMessage = 8,
        IsAS2MessageCompressed = 9,
        IsAS2MessageDuplicate = 10,
        IsAS2MessageEncrypted = 11,
        IsAS2MessageSigned = 12,
        IsAS2PayloadMessage = 13,
        MDNAsyncURI = 14,
        MessageId = 15,
        OriginalMessageId = 16,
        PreservedFileName = 17,
        SendMDN = 18
    }
}
