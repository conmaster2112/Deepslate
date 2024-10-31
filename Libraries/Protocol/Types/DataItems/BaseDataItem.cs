using System.Net;
using ConMaster.Deepslate.NBT;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.DataItems
{
    public enum DataItemValueType: uint
    {
        Byte,
        Short,
        Integer,
        Float,
        String,
        CompoudTag,
        BlockPos,
        Long,
        Vec3f,
    }
    public struct DataItem(BaseDataItemValue value) : INetworkType
    {
        public uint DataUniqueId;
        public BaseDataItemValue Value = value;
        void INetworkType.Read(ProtocolMemoryReader reader)
        {
            DataUniqueId = reader.ReadUnsignedVarInt();
            DataItemValueType type = (DataItemValueType)reader.ReadUnsignedVarInt();
            Value = type switch
            {
                DataItemValueType.Byte => new ByteDataItemValue(),
                DataItemValueType.Short => new ShortDataItemValue(),
                DataItemValueType.Float => new FloatDataItemValue(),
                DataItemValueType.Integer => new IntDataItemValue(),
                DataItemValueType.Long => new LongDataItemValue(),
                DataItemValueType.String => new StringDataItemValue(),
                DataItemValueType.CompoudTag => new CompoudTagDataItemValue(),
                DataItemValueType.BlockPos => new BlockPositionDataItemValue(),
                DataItemValueType.Vec3f => new IVec3fDataItemValue(),
                _ => throw new ProtocolViolationException("Unknown DataItemType: " + type)
            };
            reader.Read(ref Value);
        }
        void INetworkType.Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarInt(DataUniqueId);
            writer.WriteUnsignedVarInt((uint)Value.Type);
            writer.Write(ref Value);
        }
    }
    public abstract class BaseDataItemValue: INetworkType
    {
        public abstract DataItemValueType Type { get; }
        public abstract void Write(ProtocolMemoryWriter writer);
        public abstract void Read(ProtocolMemoryReader reader);
        public virtual byte TryGetByte() => (this as ByteDataItemValue)?.Value??0;
        public virtual short TryGetShort() => (this as ShortDataItemValue)?.Value ?? 0;
        public virtual int TryGetInt() => (this as IntDataItemValue)?.Value ?? 0;
        public virtual long TryGetLong() => (this as LongDataItemValue)?.Value ?? 0;
        public virtual float TryGetFloat() => (this as FloatDataItemValue)?.Value ?? 0;
        public virtual string? TryGetString() => (this as StringDataItemValue)?.Value;
        public virtual Vec3f TryGetVec3f() => (this as IVec3fDataItemValue)?.Value ?? default;
        public virtual  Vec3i  TryGetBlockPos() => (this as BlockPositionDataItemValue)?.Value ?? default;
        public virtual INBTCompoudTag? TryGetCompoudTag() => (this as CompoudTagDataItemValue)?.Value;
    }
}
