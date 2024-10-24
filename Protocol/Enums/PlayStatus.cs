namespace ConMaster.Deepslate.Protocol.Enums
{
    public enum PlayStatus: int
    {
        LoginSuccess,
        FailedClient,
        FailedServer,
        PlayerSpawn,
        FailedInvalidTenant,
        FailedVanillaEdu,
        FailedIncompatible,
        FailedServerFull,
        FailedEditorVanillaMismatch,
        FailedVanillaEditorMismatch
    }
}
