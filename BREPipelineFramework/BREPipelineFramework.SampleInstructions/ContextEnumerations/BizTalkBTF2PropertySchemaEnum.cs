namespace BREPipelineFramework.SampleInstructions
{
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/aa561250(v=bts.10).aspx for an explanation of the BizTalk Framework context properties
    /// </summary>
    public enum BizTalkBTF2PropertySchemaEnum
    {
        commitmentRct_commitmentCode = 0,
        commitmentRct_decidedAt = 1,
        commitmentRct_decision = 2,
        commitmentRct_identity = 3,
        deliveryRct_identity = 4,
        deliveryRct_receivedAt = 5,
        eps_from_address = 6,
        eps_from_address_type = 7,
        eps_to_address = 8,
        eps_to_address_type = 9,
        fault_faultactor = 10,
        fault_faultcode = 11,
        fault_faultstring = 12,
        IsReliable = 13,
        PassAckThrough = 14,
        prc_instance = 15,
        prc_type = 16,
        prop_identity = 17,
        prop_sentAt = 18,
        prop_topic = 19,
        svc_commitmentRctRqt_sendBy = 20,
        svc_commitmentRctRqt_sendTo_address = 21,
        svc_commitmentRctRqt_sendTo_address_type = 22,
        svc_deliveryRctRqt_sendBy = 23,
        svc_deliveryRctRqt_sendTo_address = 24,
        svc_deliveryRctRqt_sendTo_address_type = 25
    }
}
