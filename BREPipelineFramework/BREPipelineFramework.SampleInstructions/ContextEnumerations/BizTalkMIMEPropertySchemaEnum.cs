namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/ee267968(v=bts.10).aspx for an explanation of the MIME/SMIME context properties
    /// </summary>
    public enum BizTalkMIMEPropertySchemaEnum
    {
        ContentDescription = 0,
        ContentID = 1,
        ContentLocation = 2,
        ContentTransferEncoding = 3,
        ContentTypeSecondaryHeader = 4,
        ContentTypeSecondaryHeaderValue = 5,
        FileName = 6,
        IsMIMEEncoded = 7,
        IsMultipartRelated = 8,
        PartContentTypeSecondaryHeader = 9,
        PartContentTypeSecondaryHeaderValue = 10,
        PassThroughBTF = 11
    }
}
