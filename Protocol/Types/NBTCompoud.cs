using ConMaster.Deepslate.NBT;
using ConMaster.Deepslate.Protocol.NBT;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct EmptyNBTCompoudRoot : INBTRootCompoud
    {

        public readonly IEnumerable<KeyValuePair<string, INBTTag>> GetCompoudEntries()
        {
            return [];
        }

        public readonly void ProccessKey(ConstantNBTReader reader, string key, TagType type)
        {
            reader.SkipTag(type);
        }
    }
}
