using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class StringDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.String;
        public string Value = string.Empty;
        public override void Read(ProtocolMemoryReader reader) => Value = reader.ReadVarString();
        public override void Write(ProtocolMemoryWriter writer) => writer.WriteVarString(Value);
    }
}