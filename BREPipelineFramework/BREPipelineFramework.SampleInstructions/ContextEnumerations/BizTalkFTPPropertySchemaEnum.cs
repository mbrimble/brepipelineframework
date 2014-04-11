using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/ms962156.aspx for an explanation of the FTP context properties
    /// </summary>
    public enum BizTalkFTPPropertySchemaEnum
    {
        AfterPut = 0,
        AllocateStorage = 1,
        BeforePut = 2,
        ClientCertificateHash = 3,
        CommandLogFileName = 4,
        FtpsConnectionMode = 5,
        MaxConnections = 6,
        PassiveMode = 7,
        Password = 8,
        ReceivedFileName = 9,
        RepresentationType = 10,
        SpoolingFolder = 11,
        SSOAffiliateApplication = 12,
        UseDataProtection = 13,
        UserName = 14,
        UseSsl = 15
    }
}
