using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/ms962015.aspx for an explanation of the File context properties
    /// </summary>
    public enum BizTalkFilePropertySchemaEnum
    {
        AllowCacheOnWrite = 0,
        CopyMode = 1,
        FileCreationTime = 2,
        Password = 3,
        ReceivedFileName = 4,
        Username = 5,
        UseTempFileOnWrite = 6
    }
}
