using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct AttributeModifier : INetworkType
    {
        public string Id;
        public string Name;
        public float Amount;
        public int Operator;
        public int Operand;
        public bool IsSerializable;

        public void Read(ProtocolMemoryReader reader)
        {
            Id = reader.ReadVarString();
            Name = reader.ReadVarString();
            Amount = reader.ReadFloat();
            Operator = reader.ReadInt32();
            Operand = reader.ReadInt32();
            IsSerializable = reader.ReadBool();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Id);
            writer.WriteVarString(Name);
            writer.Write(Amount);
            writer.Write(Operator);
            writer.Write(Operand);
            writer.Write(IsSerializable);
        }
    }
}
