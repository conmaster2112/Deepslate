using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct CommandOriginData : INetworkType
    {
        public uint CommandType;
        public Guid CommandId;
        public string RequestId;
        public long PlayerUniqueId;
        public void Read(ProtocolMemoryReader reader)
        {
            CommandType = reader.ReadUnsignedVarInt();
            reader.Read(ref CommandId);
            RequestId = reader.ReadVarString();
            if(CommandType is 4 or 3)
            {
                PlayerUniqueId = reader.ReadSignedVarLong();
            }
        }

        public void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteUnsignedVarInt(CommandType);
            writer.Write(ref CommandId);
            writer.WriteVarString(RequestId);
            if (CommandType is 4 or 3) writer.WriteSignedVarLong(PlayerUniqueId);
        }
    }
}
