using ConMaster.Deepslate.Protocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct PlayerAbilitiesData : INetworkType
    {
        public long ActorUniqueId;
        public PermissionLevel PermissionLevel;
        public CommandPermissionLevel CommandPermissionLevel;
        public PlayerAbilitiesDataLayer[] Layers;
        public void Read(ProtocolMemoryReader reader)
        {
            ActorUniqueId = reader.ReadInt64();
            PermissionLevel = (PermissionLevel)reader.ReadUInt8();
            CommandPermissionLevel = (CommandPermissionLevel)reader.ReadUInt8();
            Layers = reader.ReadVarArray<PlayerAbilitiesDataLayer>();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(ActorUniqueId);
            writer.Write((byte)PermissionLevel);
            writer.Write((byte)CommandPermissionLevel);
            writer.WriteVarArray(Layers);
        }
    }
}
