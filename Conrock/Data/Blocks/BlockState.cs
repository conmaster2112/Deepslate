using ConMaster.Buffers.String;
using ConMaster.NBT;

namespace ConMaster.Bedrock.Data.Blocks
{
    public enum BlockStateKind: byte
    {
        Boolean,
        Integer,
        String
    }
    public abstract class BlockState(Utf8String id, BlockStateKind kind)
    {
        public BlockStateKind StateKind { get; private init; } = kind;
        public TagType TagType { get; private init; } = kind switch
        {
            BlockStateKind.Boolean => TagType.Byte,
            BlockStateKind.String => TagType.String,
            BlockStateKind.Integer => TagType.Int32,
            _ => throw new NotImplementedException("Unknown BlockStateType: " + kind)
        };
        public Utf8String Id { get; private init; } = id;
        public abstract void WriteAsTag(ConstantNBTWriter writer, byte indexStateValue);
        public abstract bool IsValidValue(byte indexStateValue);
    }
    public sealed class BooleanBlockState(Utf8String id): BlockState(id, BlockStateKind.Boolean)
    {
        public const byte BOOLEAN_FALSE = 0;
        public const byte BOOLEAN_TRUE = 1;
        public override bool IsValidValue(byte indexStateValue) => indexStateValue <= 1;
        public override void WriteAsTag(ConstantNBTWriter writer, byte stateValue) => writer.WriteByte(stateValue == 0 ? (byte)0 : (byte)1);
    }
    public sealed class IntBlockState(Utf8String id, IReadOnlyCollection<int> validValues) : BlockState(id, BlockStateKind.Integer)
    {
        public int[] ValidValues { get; private init; } = [.. validValues];
        public override bool IsValidValue(byte indexStateValue) => indexStateValue < ValidValues.Length;
        public override void WriteAsTag(ConstantNBTWriter writer, byte stateValue) => writer.WriteInt32(ValidValues[stateValue]);
    }
    public sealed class StringBlockState(Utf8String id, IReadOnlyCollection<Utf8String> validValues) : BlockState(id, BlockStateKind.String)
    {
        public Utf8String[] ValidValues { get; private init; } = [.. validValues];
        public override bool IsValidValue(byte indexStateValue) => indexStateValue < ValidValues.Length;
        public override void WriteAsTag(ConstantNBTWriter writer, byte stateValue) => writer.WriteString(ValidValues[stateValue]);
    }
}
