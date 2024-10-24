using ConMaster.Buffers;
using ConMaster.NBT;

namespace ConMaster.Bedrock.Data.Blocks
{
    public class BlockPermutation: INBTTag
    {
        public BlockPermutation(BlockType type, IReadOnlyCollection<byte>? permutationCombination = default)
        {
            Type = type;
            ulong permutation = 0;
            permutationCombination ??= [];
            int length = type.States.Length;
            if (length != permutationCombination.Count) throw new IndexOutOfRangeException("Number of states doesn't match number of combinations");
            if (length > 8) throw new IndexOutOfRangeException("There is too many kinds of states for this permutations, only 8 are allowed");
            int i = 0;
            foreach(byte value in permutationCombination)
            {
                BlockState state = type.States[i];
                if (!state.IsValidValue(value)) throw new InvalidDataException("Invalid combination value: " + value + " for state " + state.Id);
                permutation |= (ulong)value << (i<<3);


                //Seperate this at the end of the loop for better readability
                i++;
            }
            _permutationIndexes = permutation;
            using RentedBuffer buffer = RentedBuffer.Alloc(256); //Name + states
            int offset = 0;
            ConstantNBTWriter writer = new(new(buffer, ref offset), NBTMode.Bedrock);
            writer.WriteCompoudEntryRaw(TagType.Compoud, ""u8);
            writer.Write(this);
            
            Hash = HashAlgorithm(buffer.Span.Slice(0, offset));
        }
        public BlockType Type { get; private init; }

        // The pointer to array it self is long on all 64bit arch
        private readonly ulong _permutationIndexes; //byte[] _permutations;
        public const uint HASH_OFFSET = 0x81_1c_9d_c5;
        public readonly int Hash;
        public override int GetHashCode() => Hash.GetHashCode();
        public static int HashAlgorithm(ReadOnlySpan<byte> bytes)
        {
            uint hash = HASH_OFFSET;
            for (int i = 0; i < bytes.Length; i++)
            {
                // Set the hash to the XOR of the hash and the element.
                hash ^= bytes[i];

                // Apply the hash algorithm.
                hash +=
                    (hash << 1) + (hash << 4) + (hash << 7) + (hash << 8) + (hash << 24);
            }
            return (int)hash;
        }
        TagType INBTTag.Type => TagType.Compoud;
        public void Write(ConstantNBTWriter writer)
        {
            int l = Type.States.Length;
            writer.WriteCompoudEntry("name"u8, Type.Id);
            
            writer.WriteCompoudEntryRaw(TagType.Compoud, "states"u8);
            for (int i = 0; i < l; i++)
            {
                BlockState state = Type.States[i];
                writer.WriteCompoudEntryRaw(state.TagType, state.Id);
                state.WriteAsTag(writer, (byte)((_permutationIndexes >> (i << 3)) & 0xff));
            }
            writer.WriteEndOfCompoud();

            writer.WriteEndOfCompoud();
        }
    }
}
