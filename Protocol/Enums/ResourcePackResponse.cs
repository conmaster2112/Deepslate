namespace ConMaster.Deepslate.Protocol.Enums
{
    public enum ResourcePackResponse: byte
    {
        None,
        Refused,
        SendPacks,
        HaveAllPacks,
        Completed
    }
}
