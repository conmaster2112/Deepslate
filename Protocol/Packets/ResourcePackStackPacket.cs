using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class ResourcePackStackPacket : BasePacket<ResourcePackStackPacket>
    {
        public int PACKET_ID = 0x7;
        public override int Id => PACKET_ID;

        public bool MustAccept = false;

        public PackDefinition[] BehaviorPacks = [];
        public PackDefinition[] ResourcePacks = [];

        public string GameVersion = string.Empty;

        public ExperimentsList Experiments = new();

        public bool HasEditorExtensions = false;

        public override void Clean()
        {
            MustAccept = false;
            HasEditorExtensions = false;
            GameVersion = string.Empty;
            BehaviorPacks = [];
            ResourcePacks = [];
            Experiments = new();
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            MustAccept = reader.ReadBool();
            BehaviorPacks = reader.ReadVarArray<PackDefinition>();
            ResourcePacks = reader.ReadVarArray<PackDefinition>();
            GameVersion = reader.ReadVarString();
            reader.Read(ref Experiments);
            HasEditorExtensions = reader.ReadBool();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(MustAccept);
            writer.WriteVarArray<PackDefinition>(BehaviorPacks);
            writer.WriteVarArray<PackDefinition>(ResourcePacks);
            writer.WriteVarString(GameVersion);
            writer.Write(Experiments);
            writer.Write(HasEditorExtensions);
        }
    }
}
