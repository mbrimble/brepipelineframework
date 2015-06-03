namespace BREPipelineFramework.Helpers
{
    /// <summary>
    /// Enumeration that contains the actions that should be carried out in case a failure has been encountered during a specific Instruction
    /// </summary>
    public enum FailureActionEnum
    {
        Exception = 0,
        BlankOrDefaultValue = 1,
        Null = 2
    }
}
