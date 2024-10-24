using ConMaster.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Deepslate.NBT
{
    public delegate void Skipable(ConstantMemoryBufferReader reader);
    public abstract class NBTMode
    {
        public static NBTMode Bedrock { get; internal set; } = new BedrockNBTMode();
        public static NBTMode Java { get; internal set; } = new JavaNBTMode();
        public static NBTMode Network { get; internal set; } = new NetworkNBTMode();
        public static NBTMode Default { get; set; } = Bedrock;
        //internal Skipable[] Skipables; //Swith is still faster tho
        internal NBTMode()
        {
            
            /*Skipables = new Skipable[13];
            Skipables[(int)TagType.Byte] = SkipByte;
            Skipables[(int)TagType.Int16] = SkipInt16;
            Skipables[(int)TagType.Int32] = SkipInt32;
            Skipables[(int)TagType.Int64] = SkipInt64;
            Skipables[(int)TagType.Float32] = SkipFloat32;
            Skipables[(int)TagType.Float64] = SkipFloat64;
            Skipables[(int)TagType.ByteArray] = SkipByteArray;
            Skipables[(int)TagType.String] = SkipString;
            Skipables[(int)TagType.List] = SkipList;
            Skipables[(int)TagType.Compoud] = SkipCompoud;
            Skipables[(int)TagType.Int32Array] = SkipInt32Array;
            Skipables[(int)TagType.Int64Array] = SkipInt64Array;*/
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipByte(ConstantMemoryBufferReader reader) => reader.Offset++;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipInt16(ConstantMemoryBufferReader reader) => ReadInt16(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipInt32(ConstantMemoryBufferReader reader) => ReadInt32(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipInt64(ConstantMemoryBufferReader reader) => ReadInt64(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipFloat32(ConstantMemoryBufferReader reader) => ReadFloat32(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipFloat64(ConstantMemoryBufferReader reader) => ReadFloat64(reader);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipString(ConstantMemoryBufferReader reader)
        {
            int length = ReadStringSize(reader);
            reader.Offset += length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipByteArray(ConstantMemoryBufferReader reader)
        {
            int length = ReadArraySize(reader);
            reader.Offset += length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipList(ConstantMemoryBufferReader reader)
        {
            TagType listType = (TagType)ReadByte(reader);
            int length = ReadArraySize(reader);
            //Skipable skipable = GetSkipable((byte)listType);
            for (int i = 0; i < length; i++) Skip(listType, reader);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipCompoud(ConstantMemoryBufferReader reader)
        {
            TagType valueType = (TagType)ReadByte(reader);
            while (valueType != 0) {
                //SkipString(reader); //Key
                string key = reader.ReadSlice(ReadStringSize(reader)).AsString();
                Skip(valueType, reader);
                valueType = (TagType)ReadByte(reader);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipInt32Array(ConstantMemoryBufferReader reader)
        {
            int length = ReadArraySize(reader);
            for (int i = 0; i < length; i++) SkipInt32(reader);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipInt64Array(ConstantMemoryBufferReader reader)
        {
            int length = ReadArraySize(reader);
            for (int i = 0; i < length; i++) SkipInt64(reader);
        }

        // Check Constructor on why is this disabled
        /* 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Skipable GetSkipable(byte tag)
        {
            if (tag > 12) throw new Exception("Unknown Tag: " + tag);
            return Skipables[tag];
        }*/
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Skip(TagType type, ConstantMemoryBufferReader reader)// => GetSkipable((byte)type)(reader);
        {
            switch (type)
            {
                case TagType.Byte: SkipByte(reader); return;
                case TagType.Int16: SkipInt16(reader); return;
                case TagType.Int32: SkipInt32(reader); return;
                case TagType.Int64: SkipInt64(reader); return;
                case TagType.Float32: SkipFloat32(reader); return;
                case TagType.Float64: SkipFloat64(reader); return;
                case TagType.ByteArray: SkipByteArray(reader); return;
                case TagType.String: SkipString(reader); return;
                case TagType.List: SkipList(reader); return;
                case TagType.Compoud: SkipCompoud(reader); return;
                case TagType.Int32Array: SkipInt32Array(reader); return;
                case TagType.Int64Array: SkipInt64Array(reader); return;
                default:
                    throw new Exception("Invalid Tag to skip: " + type);
            }
        }


        public abstract int ReadArraySize(ConstantMemoryBufferReader reader);
        public abstract int ReadStringSize(ConstantMemoryBufferReader reader);
        public abstract byte ReadByte(ConstantMemoryBufferReader reader);
        public abstract short ReadInt16(ConstantMemoryBufferReader reader);
        public abstract int ReadInt32(ConstantMemoryBufferReader reader);
        public abstract long ReadInt64(ConstantMemoryBufferReader reader);
        public abstract float ReadFloat32(ConstantMemoryBufferReader reader);
        public abstract double ReadFloat64(ConstantMemoryBufferReader reader);


        public abstract void WriteArraySize(ConstantMemoryBufferWriter writer, int size);
        public abstract void WriteStringSize(ConstantMemoryBufferWriter writer, int size);
        public abstract void WriteByte(ConstantMemoryBufferWriter writer, byte value);
        public abstract void WriteInt16(ConstantMemoryBufferWriter writer, short value);
        public abstract void WriteInt32(ConstantMemoryBufferWriter writer, int value);
        public abstract void WriteInt64(ConstantMemoryBufferWriter writer, long value);
        public abstract void WriteFloat32(ConstantMemoryBufferWriter writer, float value);
        public abstract void WriteFloat64(ConstantMemoryBufferWriter writer, double value);
    }
}
