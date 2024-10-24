using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct NetworkPermission : INetworkType
    {
        public bool ServerAuthSoundEnabled;
        public void Read(ProtocolMemoryReader reader)
        {
            ServerAuthSoundEnabled = reader.ReadBool();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(ServerAuthSoundEnabled);
        }
    }
}
