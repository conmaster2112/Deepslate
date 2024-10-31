using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class UpdateAttributesPacket: BasePacket<UpdateAttributesPacket>
    {
        public const int PACKET_ID = 29;
        public override int Id => PACKET_ID;
        public ulong EntityRuntimeId;
        public IReadOnlyCollection<IAttributeComponent> AttributeList = [];
        public ulong CurrentTick;

        public override void Clean()
        {
            EntityRuntimeId = default;
            AttributeList = []; 
            CurrentTick = default;
        }

        public override void Read(ProtocolMemoryReader reader)
        {
            throw new NotImplementedException();
            /*
            EntityRuntimeId = reader.ReadUnsignedVarLong();
            CurrentTick = reader.ReadUnsignedVarLong();
            */
        }

        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarLong(EntityRuntimeId);
            writer.WriteVarArray(AttributeList);
            writer.WriteUnsignedVarLong(CurrentTick);
        }
    }
    public interface IAttributeComponent : INetworkType
    {
        public string Id { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float DefaultMinValue { get; set; }
        public float DefaultMaxValue { get; set; }
        public float CurrentValue { get; set; }
        public float DefaultValue { get; set; }
        public IReadOnlyCollection<AttributeModifier> Modifiers { get; set; }

        void INetworkType.Read(ProtocolMemoryReader reader)
        {
            MinValue = reader.ReadFloat();
            MaxValue = reader.ReadFloat();
            CurrentValue = reader.ReadFloat();
            DefaultMinValue = reader.ReadFloat();
            DefaultMaxValue = reader.ReadFloat();
            DefaultValue = reader.ReadFloat();
            Id = reader.ReadVarString();
            Modifiers = reader.ReadVarArray<AttributeModifier>();
        }
        void INetworkType.Write(ProtocolMemoryWriter writer)
        {
            writer.Write(MinValue);
            writer.Write(MaxValue);
            writer.Write(CurrentValue);

            writer.Write(DefaultMinValue);
            writer.Write(DefaultMaxValue);
            writer.Write(DefaultValue);

            writer.WriteVarString(Id);
            Console.WriteLine("Serialized with: " + Id);
            writer.WriteVarArray(Modifiers);
        }
    }
    public struct AttribtuteComponent: IAttributeComponent
    {
        public string Id { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float CurrentValue { get; set; }
        public float DefaultValue { get; set; }
        public IReadOnlyCollection<AttributeModifier> Modifiers;

        public float DefaultMinValue { get; set; }
        public float DefaultMaxValue { get; set; }
        IReadOnlyCollection<AttributeModifier> IAttributeComponent.Modifiers { get => Modifiers; set => Modifiers = []; }
    }
}
