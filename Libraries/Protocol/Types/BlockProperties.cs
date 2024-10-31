using ConMaster.Deepslate.NBT;

using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.NBT;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct BlockPropertyDefinition : INetworkType
    {
        public string Name;
        public BlockDefinition Definition;

        public void Read(ProtocolMemoryReader reader)
        {
            Name = reader.ReadVarString();
            Definition ??= new();
            reader.Read(ref Definition);
        }

        public void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Name);
            Definition ??= new();
            writer.Write(Definition);
        }
    }
    public class BlockDefinition : INBTRootCompoud
    {
        public IEnumerable<KeyValuePair<string, INBTTag>> GetCompoudEntries()
        {
            return [];
        }
        public void ProccessKey(ConstantNBTReader reader, string key, TagType type)
        {
            reader.SkipTag(type);
        }
    }
}
