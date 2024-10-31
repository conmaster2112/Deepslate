using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct GameRule: INetworkType
    {
        public string Name;
        public bool IsEditable;
        public uint Type;
        public int Value;
        public readonly bool IsBoolValue => Type == 1;

        public void Read(ProtocolMemoryReader reader)
        {
            Name = reader.ReadVarString();
            IsEditable = reader.ReadBool();
            Type = reader.ReadUnsignedVarInt();
            if (IsBoolValue) Value = reader.ReadBool() ? 1 : 0;
            else Value = reader.ReadSignedVarInt();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Name);
            writer.Write(IsEditable); 
            writer.WriteUnsignedVarInt(Type);
            if(IsBoolValue) writer.Write(Value != 0);
            else writer.WriteSignedVarInt(Value);
        }
    }
}
