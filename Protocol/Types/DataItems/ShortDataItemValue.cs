using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class ShortDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.Short;
        public short Value;
        public override void Read(ProtocolMemoryReader reader) => Value = reader.ReadInt16();
        public override void Write(ProtocolMemoryWriter writer) => writer.Write(Value);
    }
}

