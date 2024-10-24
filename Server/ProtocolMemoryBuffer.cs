using ConMaster.Buffers;
using System.Text;

namespace ConMaster.Deepslate.Network
{
    public readonly ref struct ProtocolMemoryReader(ConstantMemoryBufferReader reader)
    {
        public static implicit operator ProtocolMemoryReader(ConstantMemoryBufferReader reader) => new(reader);
        public readonly ConstantMemoryBufferReader Reader = reader;
        public readonly int ReadVarLength()=>(int)Reader.ReadUVarInt32();
        public readonly int ReadLength32() => (int)Reader.ReadUInt32();
        public readonly int ReadLength16() => (int)Reader.ReadUInt16();

        public readonly ReadOnlySpan<byte> ReadSlice(int length) => Reader.ReadSlice(length);

        public readonly int ReadSignedVarInt() => Reader.ReadVarInt32();
        public readonly uint ReadUnsignedVarInt() => Reader.ReadUVarInt32();
        public readonly long ReadSignedVarLong() => Reader.ReadVarInt64();
        public readonly ulong ReadUnsignedVarLong() => Reader.ReadUVarInt64();

        public readonly byte ReadUInt8()=>Reader.ReadUInt8();
        public readonly bool ReadBool() => Reader.ReadBool();
        public readonly short ReadInt16() => Reader.ReadInt16();
        public readonly ushort ReadUInt16() => Reader.ReadUInt16();
        public readonly int ReadInt32() => Reader.ReadInt32();
        public readonly uint ReadUInt32() => Reader.ReadUInt32();
        public readonly int ReadInt32BE() => Reader.ReadInt32BigEndian();
        public readonly uint ReadUInt32BE() => Reader.ReadUInt32BigEndian();
        public readonly long ReadInt64() => Reader.ReadInt64();
        public readonly ulong ReadUInt64() => Reader.ReadUInt64();
        public readonly float ReadFloat() => Reader.ReadFloat32();
        public readonly double ReadDouble() => Reader.ReadFloat64();
        public readonly string ReadVarString() => Encoding.UTF8.GetString(Reader.ReadSlice(ReadVarLength()));
        public readonly string ReadString32() => Encoding.UTF8.GetString(Reader.ReadSlice(ReadLength32()));
        public readonly void Read<T>(ref T value) where T : INetworkType => value.Read(this);
        public readonly void Read(ref Guid value) => value = new(Reader.ReadSlice(16));
        public readonly T[] ReadSpan<T>(int length) where T : struct, INetworkType
        {
            T[] array = new T[length];
            for (int i = 0; i < length; i++) array[i].Read(this);
            return array;
        }
        public readonly void ReadSpan<T>(ICollection<T> collection, int length) where T : struct, INetworkType
        {
            T value = default;
            for (int i = 0; i < length; i++)
            {
                value.Read(this);
                collection.Add(value);
            }
        }
        public readonly T[] ReadVarArray<T>() where T : struct, INetworkType => ReadSpan<T>(ReadVarLength());
        public readonly T[] ReadArray32<T>() where T : struct, INetworkType => ReadSpan<T>(ReadLength32());
        public readonly T[] ReadArray16<T>() where T : struct, INetworkType => ReadSpan<T>(ReadLength16());
        public readonly void ReadVarArray<T>(ICollection<T> collection) where T : struct, INetworkType => ReadSpan<T>(collection,ReadVarLength());
        public readonly void ReadArray32<T>(ICollection<T> collection) where T : struct, INetworkType => ReadSpan<T>(collection, ReadLength32());
        public readonly void ReadArray16<T>(ICollection<T> collection) where T : struct, INetworkType => ReadSpan<T>(collection, ReadLength16());
        public readonly int ReadPacketId ()=> ReadVarLength();
    }
    public readonly ref struct ProtocolMemoryWriter(ConstantMemoryBufferWriter writer)
    {
        public static implicit operator ProtocolMemoryWriter(ConstantMemoryBufferWriter writer) => new(writer);
        public readonly ConstantMemoryBufferWriter Writer = writer;
        public readonly void WriteVarLength(int length) => Writer.WriteUVarInt32((uint)length);
        public readonly void WriteLength32(int length) => Writer.Write((uint)length);
        public readonly void WriteLength16(int length) => Writer.Write((ushort)length);

        public readonly void WriteSignedVarInt(int value) => Writer.WriteVarInt32(value);
        public readonly void WriteUnsignedVarInt(uint value) => Writer.WriteUVarInt32(value);
        public readonly void WriteSignedVarLong(long value) => Writer.WriteVarInt64(value);
        public readonly void WriteUnsignedVarLong(ulong value) => Writer.WriteUVarInt64(value);

        public readonly void Write(byte v) => Writer.Write(v);
        public readonly void Write(bool v) => Writer.Write(v);
        public readonly void Write(short v) => Writer.Write(v);
        public readonly void Write(ushort v) => Writer.Write(v);
        public readonly void Write(int v) => Writer.Write(v);
        public readonly void Write(uint v) => Writer.Write(v);
        public readonly void WriteBE(int v) => Writer.WriteBigEndian(v);
        public readonly void WriteBE(uint v) => Writer.WriteBigEndian(v);
        public readonly void Write(long v) => Writer.Write(v);
        public readonly void Write(ulong v) => Writer.Write(v);
        public readonly void Write(float v) => Writer.Write(v);
        public readonly void Write(double v) => Writer.Write(v);

        public readonly void WriteVarString(string? v)
        {
            if (string.IsNullOrEmpty(v))
            {
                WriteVarLength(0);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(v);
                WriteVarLength(bytes.Length);
                Writer.Write(bytes);
            }
        }
        public readonly void WriteString32(string? v)
        {
            if (string.IsNullOrEmpty(v))
            {
                WriteLength32(0);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(v);
                WriteLength32(bytes.Length);
                Writer.Write(bytes);
            }
        }
        public readonly void WriteVarStringRaw(ReadOnlySpan<byte> v)
        {
            WriteVarLength(v.Length);
            Writer.Write(v);
        }
        public readonly void WriteString32Raw(ReadOnlySpan<byte> v)
        {
            WriteLength32(v.Length);
            Writer.Write(v);
        }

        public readonly void Write(ref Guid value) => Writer.Write(value.ToByteArray());
        // Use this its faster, no copies created for structure types, that may be huge
        public readonly void Write<T>(ref T value) where T : INetworkType
        {
            value.Write(this);
        }
        // Compatibility with old packets
        public readonly void Write<T>(T value) where T : INetworkType
        {
            value.Write(this);
        }
        public readonly void WriteVarArray<T>(IReadOnlyCollection<T>? collection) where T : INetworkType
        {
            if (collection == null)
            {
                WriteVarLength(0);
                return;
            }
            WriteVarLength(collection.Count);
            foreach (var item in collection) item.Write(this);
        }
        public readonly void WriteArray32<T>(IReadOnlyCollection<T>? collection) where T : INetworkType
        {
            if (collection == null)
            {
                WriteLength32(0);
                return;
            }
            WriteLength32(collection.Count);
            foreach (var item in collection) item.Write(this);
        }
        public readonly void WriteArray16<T>(IReadOnlyCollection<T>? collection) where T : INetworkType
        {
            if (collection == null)
            {
                WriteLength16(0);
                return;
            }
            WriteLength16(collection.Count);
            foreach (var item in collection) item.Write(this);
        }
        public readonly void WritePackedId(int id) => Writer.WriteUVarInt32((uint)id);
    }
}
