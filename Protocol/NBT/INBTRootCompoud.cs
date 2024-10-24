using ConMaster.Buffers;
using ConMaster.Deepslate.NBT;
using ConMaster.Deepslate.Network;
using System.Text;

namespace ConMaster.Deepslate.Protocol.NBT
{
    public interface INBTRootCompoud: INetworkNBT
    {
        TagType INBTTag.Type => TagType.Compoud;
        public void ProccessKey(ConstantNBTReader reader, string key, TagType type);
        public IEnumerable<KeyValuePair<string, INBTTag>> GetCompoudEntries();
        void INetworkNBT.Read(ConstantNBTReader reader)
        {
            reader.ReadCompoudEntry(out TagType type, out ReadOnlySpan<byte> key);
            if (type != TagType.Compoud) throw new Exception("Compoud Tag Type expected but recieved: " + type);
            while (reader.ReadCompoudEntry(out type, out key)) ProccessKey(reader, key.AsString(), type);
        }
        void INBTTag.Write(ConstantNBTWriter writer)
        {
            writer.WriteCompoudEntryRaw(TagType.Compoud, []);
            foreach(var kv in GetCompoudEntries())
            {
                writer.WriteCompoudEntryRaw(kv.Value.Type, kv.Key.GetBytes());
                writer.Write(kv.Value);
            }
            writer.WriteEndOfCompoud();
        }
    }
}
