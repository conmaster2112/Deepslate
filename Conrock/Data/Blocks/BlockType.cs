using ConMaster.Bedrock.Data.Types;

namespace ConMaster.Bedrock.Data.Blocks
{
    public class BlockType : Type<BlockType>
    {
        public BlockState[] States { get; private init; }
        public BlockType(string id, IReadOnlyCollection<BlockState>? states = default) : base(id)
        {
            states ??= [];
            int length = states.Count, index = 0;
            States = GC.AllocateUninitializedArray<BlockState>(length);
            foreach (var state in states) States[index++] = state;
        }
    }
}
