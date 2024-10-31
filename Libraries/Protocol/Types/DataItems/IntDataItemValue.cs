using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class IntDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.Integer;
        public int Value;
        public override void Read(ProtocolMemoryReader reader) => Value = reader.ReadSignedVarInt();
        public override void Write(ProtocolMemoryWriter writer) => writer.WriteSignedVarInt(Value);
    }
}

