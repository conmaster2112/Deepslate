using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class LongDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.Long;
        public long Value;
        public override void Read(ProtocolMemoryReader reader) => Value = reader.ReadSignedVarLong();
        public override void Write(ProtocolMemoryWriter writer)
        {
            Console.WriteLine(Value);
            writer.WriteSignedVarLong(Value);
        }
    }
}

