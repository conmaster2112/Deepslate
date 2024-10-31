using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct SyncPlayerMovementSettings: INetworkType
    {
        public int AuthorityMode;
        public int RewindHistorySize;
        public bool ServerAuthorityBlockBreaking;

        public void Read(ProtocolMemoryReader reader)
        {
            AuthorityMode = reader.ReadSignedVarInt();
            RewindHistorySize = reader.ReadSignedVarInt();
            ServerAuthorityBlockBreaking = reader.ReadBool();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(AuthorityMode);
            writer.WriteSignedVarInt(RewindHistorySize);
            writer.Write(ServerAuthorityBlockBreaking);
        }
    }
}
