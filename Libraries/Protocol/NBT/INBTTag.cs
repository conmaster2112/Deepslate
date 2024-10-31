using ConMaster.Deepslate.NBT;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.NBT
{
    public interface INetworkNBT : INetworkType, INBTTag
    {
        public void Read(ConstantNBTReader reader);
        void INetworkType.Read(ProtocolMemoryReader reader)
        {
            Read(new ConstantNBTReader(reader.Reader, NBTMode.Network));
        }
        void INetworkType.Write(ProtocolMemoryWriter writer)
        {
            Write(new ConstantNBTWriter(writer.Writer, NBTMode.Network));
        }
    }
}
