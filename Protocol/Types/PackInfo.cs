using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct PackInfo: INetworkType
    {
        public string Uuid;
        public string Version;
        public uint Size;
        public string ContentKey;
        public string SubpackName;
        public string ContentIdentity;
        public bool HasScripts;
        public bool IsAddon;
        public void Read(ProtocolMemoryReader reader)
        {
            Uuid = reader.ReadVarString();
            Version = reader.ReadVarString();
            Size = reader.ReadUInt32();
            ContentKey = reader.ReadVarString();
            SubpackName = reader.ReadVarString();
            ContentIdentity = reader.ReadVarString();
            HasScripts = reader.ReadBool();
            IsAddon = reader.ReadBool();
        }
        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Uuid);
            writer.WriteVarString(Version);
            writer.Write(Size);
            writer.WriteVarString(ContentKey);
            writer.WriteVarString(SubpackName);
            writer.WriteVarString(ContentIdentity);
            writer.Write(HasScripts);
            writer.Write(IsAddon);
        }
    }
}
