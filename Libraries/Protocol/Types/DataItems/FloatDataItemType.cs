using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class FloatDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.Float;
        public float Value;
        public override void Read(ProtocolMemoryReader reader) => Value = reader.ReadFloat();
        public override void Write(ProtocolMemoryWriter writer) => writer.Write(Value);
    }
}

