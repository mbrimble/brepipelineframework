namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/aa578516.aspx for an explanation of the Failed Message Routing context properties
    /// </summary>
    public enum BizTalkErrorReportPropertySchemaEnum
    {
        Description = 0,
        ErrorType = 1,
        FailureAdapter = 2,
        FailureCategory = 3,
        FailureCode = 4,
        FailureInstanceID = 5,
        FailureMessageID = 6,
        FailureTime = 7,
        InboundTransportLocation = 8,
        MessageType = 9,
        OutboundTransportLocation = 10,
        ProcessingServer = 11,
        ReceivePortName = 12,
        RoutingFailureReportID = 13,
        SendPortName = 14
    }
}
