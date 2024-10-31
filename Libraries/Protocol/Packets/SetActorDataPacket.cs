using ConMaster.Deepslate.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Types.DataItems;

namespace ConMaster.Deepslate.Protocol.Packets
{
    public class SetActorDataPacket: BasePacket<SetActorDataPacket>
    {
        public const int PACKET_ID = 39;
        public override int Id => PACKET_ID;
        public ulong EntityRuntimeId;
        public IReadOnlyCollection<DataItem> Items = [];
        public PropertySyncData SyncData;
        public ulong CurrentTick;


        public override void Clean()
        {
            EntityRuntimeId = default;
            Items = [];
            SyncData = default;
            CurrentTick = default;
        }
        public override void Read(ProtocolMemoryReader reader)
        {
            EntityRuntimeId = reader.ReadUnsignedVarLong();
            Items = reader.ReadVarArray<DataItem>();
            reader.Read(ref SyncData);
            CurrentTick = reader.ReadUnsignedVarLong();
        }
        public override void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarLong(EntityRuntimeId);
            writer.WriteVarArray(Items);
            writer.Write(ref SyncData);
            writer.WriteUnsignedVarLong(CurrentTick);
        }
    }
}
