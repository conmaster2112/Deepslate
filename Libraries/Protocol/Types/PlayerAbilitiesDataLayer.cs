using ConMaster.Bedrock.Enums;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct PlayerAbilitiesDataLayer : INetworkType
    {
        public AbilityLayerType LayerType;
        public AbilityFlag AbilitiesAllowed;
        public AbilityFlag AbilitiesEnabled;
        public float FlySpeed;
        public float WalkSpeed;

        public void Read(ProtocolMemoryReader reader)
        {
            LayerType = (AbilityLayerType)reader.ReadUInt16();
            AbilitiesAllowed = (AbilityFlag)reader.ReadUInt32();
            AbilitiesEnabled = (AbilityFlag)reader.ReadUInt32();
            FlySpeed = reader.ReadFloat();
            WalkSpeed = reader.ReadFloat();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write((ushort)LayerType);
            writer.Write((uint)AbilitiesAllowed);
            writer.Write((uint)AbilitiesEnabled);
            writer.Write(FlySpeed);
            writer.Write(WalkSpeed);
        }
    }
}
