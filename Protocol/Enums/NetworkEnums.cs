using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Enums
{
    public static class NetworkEnums
    {
        public static void Write(ProtocolMemoryWriter writer, GameMode mode) => writer.WriteSignedVarInt((int)(mode));
        public static void Write(ProtocolMemoryWriter writer, Difficulty value) => writer.WriteSignedVarInt((int)value);
        public static void Write(ProtocolMemoryWriter writer, DisconnectReason value) => writer.WriteSignedVarInt((int)value);
        public static void Write(ProtocolMemoryWriter writer, MoveMode value) => writer.Write((byte)value);
        public static void Write(ProtocolMemoryWriter writer, PermissionLevel value) => writer.Write((byte)value);
        public static void Write(ProtocolMemoryWriter writer, CommandPermissionLevel value) => writer.Write((byte)value);
        
        public static GameMode ReadGameMode(ProtocolMemoryReader writer) => (GameMode)writer.ReadSignedVarInt();
        public static Difficulty ReadDifficulty(ProtocolMemoryReader reader) => (Difficulty)reader.ReadSignedVarInt();
        public static DisconnectReason ReadDisconnectReason(ProtocolMemoryReader reader) => (DisconnectReason)reader.ReadSignedVarInt();
        public static MoveMode ReadMoveMode(ProtocolMemoryReader reader) => (MoveMode)reader.ReadUInt8();
        public static PermissionLevel ReadPermissionLevel(ProtocolMemoryReader reader) => (PermissionLevel)reader.ReadUInt8();
        public static CommandPermissionLevel ReadCommandPermissionLevel(ProtocolMemoryReader reader) => (CommandPermissionLevel)reader.ReadUInt8();
    }
}
