using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class ByteDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.Byte;
        public byte Value;
        public override void Read(ProtocolMemoryReader reader) => Value = reader.ReadUInt8();
        public override void Write(ProtocolMemoryWriter writer) => writer.Write(Value);
    }
}
