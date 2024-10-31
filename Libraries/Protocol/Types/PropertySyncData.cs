using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct PropertySyncData: INetworkType
    {
        public IReadOnlyCollection<IntPropertyElement> IntProperties;
        public IReadOnlyCollection<FloatPropertyElement> FloatProperties;

        public void Read(ProtocolMemoryReader reader)
        {
            IntProperties = reader.ReadVarArray<IntPropertyElement>();
            FloatProperties = reader.ReadVarArray<FloatPropertyElement>();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarArray(IntProperties);
            writer.WriteVarArray(FloatProperties);
        }
    }
    public struct IntPropertyElement: INetworkType
    {
        public uint Id;
        public int Value;

        public void Read(ProtocolMemoryReader reader)
        {
            Id = reader.ReadUnsignedVarInt();
            Value = reader.ReadSignedVarInt();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarInt(Id);
            writer.WriteSignedVarInt(Value);
        }
    }
    public struct FloatPropertyElement: INetworkType
    {
        public uint Id;
        public float Value;
        public void Read(ProtocolMemoryReader reader)
        {
            Id = reader.ReadUnsignedVarInt();
            Value = reader.ReadFloat();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarInt(Id);
            writer.Write(Value);
        }
    }
}
