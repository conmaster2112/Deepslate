using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct ExperimentsList : INetworkType
    {
        public static readonly ExperimentsList Instance = new();
        public Experiment[] Experiments;
        public bool WerePrevioslyEnabled;

        public void Read(ProtocolMemoryReader reader)
        {
            Experiments = reader.ReadArray32<Experiment>();
            WerePrevioslyEnabled = reader.ReadBool();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteArray32(Experiments);
            writer.Write(WerePrevioslyEnabled);
        }
    }
    //Uint32 array
    public struct Experiment() : INetworkType
    {
        public string Name = string.Empty;
        public bool Enabled = false;
        public void Read(ProtocolMemoryReader reader)
        {
            Name = reader.ReadVarString();
            Enabled = reader.ReadBool();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Name);
            writer.Write(Enabled);
        }
    }
}
