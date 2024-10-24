using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConMaster.Buffers
{
    public static partial class BufferExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AsString(this ReadOnlySpan<byte> buffer, Encoding? encoding = default) {
            encoding ??= Encoding.UTF8;
            return encoding.GetString(buffer);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AsString(this Span<byte> buffer, Encoding? encoding = default)
        {
            return (encoding??Encoding.UTF8).GetString(buffer);
        }
        public static byte[] GetBytes(this string text, Encoding? encoding = default)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(text);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> ReadString32Raw(this ConstantMemoryBufferReader reader)
        {
            return reader.ReadSlice(reader.ReadInt32());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> ReadVarStringRaw(this ConstantMemoryBufferReader reader)
        {
            return reader.ReadSlice((int)reader.ReadUVarInt32());
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadVarString(this ConstantMemoryBufferReader reader) => ReadVarStringRaw(reader).AsString();




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString32Raw(this ConstantMemoryBufferWriter writer, ReadOnlySpan<byte> bytes)
        {
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRawString32BigEndian(this ConstantMemoryBufferWriter writer, ReadOnlySpan<byte> bytes)
        {
            writer.WriteBigEndian(bytes.Length);
            writer.Write(bytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRawVarString(this ConstantMemoryBufferWriter writer, ReadOnlySpan<byte> bytes)
        {
            writer.WriteUVarInt32((uint)bytes.Length);
            writer.Write(bytes);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString32(this ConstantMemoryBufferWriter writer, string text) => WriteString32Raw(writer, text.GetBytes());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteString32BigEndian(this ConstantMemoryBufferWriter writer, string text) => WriteRawString32BigEndian(writer, text.GetBytes());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteVarString(this ConstantMemoryBufferWriter writer, string text) => WriteRawVarString(writer, text.GetBytes());
    }
}
