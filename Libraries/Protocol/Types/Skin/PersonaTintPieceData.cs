using System.Text.Json;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.Skin
{
    public struct PersonaTintPieceData() : INetworkType
    {
        public static readonly PersonaTintPieceData Empty = new();
        public string Type = string.Empty;
        public string[] Colors = [];
        public void Read(ProtocolMemoryReader reader)
        {
            Type = reader.ReadVarString();
            uint length = reader.ReadUInt32();
            Colors = new string[length];
            for (int i = 0; i < length; i++) Colors[i] = reader.ReadVarString();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Type);
            writer.Write((uint)Colors.Length);
            for (int i = 0; i < Colors.Length; i++) writer.WriteVarString(Colors[i]);
        }
        public static PersonaTintPieceData FromJson(JsonElement element)
        {
            if(element.ValueKind != JsonValueKind.Object)
            {
                Console.WriteLine("Is not JSON object at PersonaTintPieceData::FromJson");
                return Empty;
            }
            PersonaTintPieceData result = new();
            JsonElement pieceType = element.GetProperty("PieceType");
            if (pieceType.ValueKind != JsonValueKind.String)
            {
                Console.WriteLine("PieceType is not JSON string at PersonaTintPieceData::FromJson");
                return Empty;
            }
            result.Type = pieceType.GetString()!;


            JsonElement colors = element.GetProperty("Colors");
            if (colors.ValueKind != JsonValueKind.Array)
            {
                Console.WriteLine("Colors is not JSON Array at PersonaTintPieceData::FromJson");
                return Empty;
            }
            JsonElement[] elements = colors.EnumerateArray().ToArray();
            result.Colors = new string[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                JsonElement color = elements[i];
                if (color.ValueKind != JsonValueKind.String)
                {
                    Console.WriteLine("color is not JSON string at PersonaTintPieceData::FromJson");
                    return Empty;
                }
                result.Colors[i] = color.GetString()!;
            }
            return result;
        }
    }
}
