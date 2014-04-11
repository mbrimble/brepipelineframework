using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://technet.microsoft.com/en-us/library/aa559096.aspx for an explanation of the XML and Flat File context properties
    /// </summary>
    public enum BizTalkXMLNORMPropertySchemaEnum
    {
        AddXMLDeclaration = 0,
        AllowUnrecognizedMessage = 1,
        BamTrackingOnly = 2,
        DocumentSpecName = 3,
        EnvelopeSpecName = 4,
        FlatFileHeaderDocument = 5,
        HeaderSpecName = 6,
        InboundPropertiesTracked = 7,
        PreserveBom = 8,
        ProcessingInstruction = 9,
        ProcessingInstructionOption = 10,
        ProcessingInstructionScope = 11,
        PromotePropertiesOnly = 12,
        RecoverableInterchangeProcessing = 13,
        SourceCharset = 14,
        TargetCharset = 15,
        TrailerSpecName = 16
    }
}
