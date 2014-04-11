using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://technet.microsoft.com/en-us/library/aa578208.aspx for an explanation of the SOAP context properties
    /// </summary>
    public enum BizTalkSOAPPropertySchemaEnum
    {
        AffiliateApplicationName = 0,
        AssemblyName = 1,
        AuthenticationScheme = 2,
        bts_soap_properties = 3,
        ClientCertificate = 4,
        ClientConnectionTimeout = 5,
        MethodName = 6,
        Password = 7,
        ProxyAddress = 8,
        ProxyPassword = 9,
        ProxyPort = 10,
        ProxyUsername = 11,
        TypeName = 12,
        UnknownHeaders = 13,
        UseHandlerSetting = 14,
        UseProxy = 15,
        UserDefined = 16,
        Username = 17,
        UseSoap12 = 18,
        UseSSO = 19
    }
}
