using ConMaster.Bedrock.Network;
using ConMaster.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Bedrock.Base
{
    public struct EntityRotation: INetworkType
    {
        public float Pitch;
        public float Yaw;

        public void Read(ProtocolMemoryReader reader)
        {
            Pitch = reader.ReadFloat();
            Yaw = reader.ReadFloat();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(Pitch);
            writer.Write(Yaw);
        }
        public readonly override string ToString()
        {
            return $"<{Pitch}, {Yaw}>";
        }
    }
}
