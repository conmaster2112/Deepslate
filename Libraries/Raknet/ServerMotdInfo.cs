namespace ConMaster.Raknet
{
    public struct ServerMotdInfo
    {
        public bool IsEducationEdition;
        public string Name;
        public int ProtocolVersion;
        public string GameVersion;

        public int MaxPlayerCount;
        public int CurrentPlayerCount;
        public string LevelName;
    }
}
