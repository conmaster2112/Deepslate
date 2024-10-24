using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.NBT;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public sealed class CompoudTagDataItemValue : BaseDataItemValue
    {
        public override DataItemValueType Type => DataItemValueType.CompoudTag;
        public INBTCompoudTag? Value = default;
        public override void Read(ProtocolMemoryReader reader)
        {
            throw new NotImplementedException();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
