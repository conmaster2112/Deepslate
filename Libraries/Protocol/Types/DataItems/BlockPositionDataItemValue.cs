using ConMaster.Deepslate.Protocol.Types;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class BlockPositionDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.BlockPos;
        public  Vec3i  Value = default;
        public override void Read(ProtocolMemoryReader reader) => reader.Read(ref Value);
        public override void Write(ProtocolMemoryWriter writer) => writer.Write(ref Value);
    }
}
