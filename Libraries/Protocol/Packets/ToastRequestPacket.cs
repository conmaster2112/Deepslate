using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Buffers;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class ToastRequestPacket : BasePacket<ToastRequestPacket>
    {
        public const int PACKET_ID = 186;
        public override int Id => PACKET_ID;

        public string? Title = default;
        public string? Content = default;
        public override void Clean()
        {
            Title = default;
            Content = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            Title = reader.ReadVarString();
            Content = reader.ReadVarString();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Title);
            writer.WriteVarString(Content);
        }
        public static ToastRequestPacket From(string title, string content)
        {
            ToastRequestPacket toast = Create();
            toast.Title = title;
            toast.Content = content;
            return toast;
        }
    }
}
