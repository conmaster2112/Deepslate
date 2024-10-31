using ConMaster.Deepslate.Protocol.Types;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class IVec3fDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.Vec3f;
        public Vec3f Value = default;
        public override void Read(ProtocolMemoryReader reader) => reader.Read(ref Value);
        public override void Write(ProtocolMemoryWriter writer) => writer.Write(ref Value);
    }
}
