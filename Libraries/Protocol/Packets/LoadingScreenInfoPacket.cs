using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Buffers;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class LoadingScreenInfoPacket : BasePacket<LoadingScreenInfoPacket>
    {
        public const int PACKET_ID = 312;
        public override int Id => PACKET_ID;

        public int LoadingStateId = 0;
        public bool HasLoadingId = false;
        public uint LoadingScreenId = uint.MaxValue;
        public override void Clean()
        {
            LoadingStateId = default;
            HasLoadingId = default;
            LoadingScreenId = 0;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            LoadingStateId = reader.ReadSignedVarInt();
            if(HasLoadingId = reader.ReadBool()) LoadingScreenId = reader.ReadUInt32();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteSignedVarInt(LoadingStateId);
            if (HasLoadingId)
            {
                writer.Write(true);
                writer.Write(LoadingScreenId);
            }
        }
    }
}
