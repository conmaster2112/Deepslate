using ConMaster.Deepslate.Protocol.Types;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class ResourcePacksInfoPacket : BasePacket<ResourcePacksInfoPacket>
    {
        public const int PACKET_ID = 6;
        public override int Id => PACKET_ID;
        public bool MustAccept = false;
        public bool HasAddons = false;
        public bool HasScripts = false;
        //public bool ForceServerPacks = false;
        //public BehaviorPackInfo[]? BehaviorPacks = null;
        public ResourcePackInfo[]? ResourcePacks = null;
        //public PackLink[]? CndUrls = null;

        public override void Clean() // Struct types doesn't need release
        {
            MustAccept = false;
            HasAddons = false;
            HasScripts = false;
            //ForceServerPacks = false;
            //BehaviorPacks = [];
            ResourcePacks = [];
            //CndUrls = [];
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            MustAccept = reader.ReadBool();
            HasAddons = reader.ReadBool();
            HasScripts = reader.ReadBool();
            //ForceServerPacks = reader.ReadBool();

            //BehaviorPacks = reader.ReadArray16<BehaviorPackInfo>();
            ResourcePacks = reader.ReadArray16<ResourcePackInfo>();
            //CndUrls = reader.ReadVarArray<PackLink>();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(MustAccept);
            writer.Write(HasAddons);
            writer.Write(HasScripts);

            //writer.WriteArray16(BehaviorPacks);
            writer.WriteArray16(ResourcePacks);
            //writer.WriteVarArray(CndUrls);
        }
    }
}
