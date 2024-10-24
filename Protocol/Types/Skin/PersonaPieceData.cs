using System.Text.Json;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.Skin
{
    public struct PersonaPieceData() : INetworkType
    {
        public static readonly PersonaPieceData Empty = new();
        public Guid Id = Guid.Empty;
        public string Type = string.Empty;
        public Guid PackId = Guid.Empty;
        public Guid ProductId = Guid.Empty;
        public bool IsDefault = false;
        public void Read(ProtocolMemoryReader reader)
        {
            Guid.TryParse(reader.ReadVarString(), out Id);
            Type = reader.ReadVarString();
            Guid.TryParse(reader.ReadVarString(), out PackId);
            IsDefault = reader.ReadBool();
            Guid.TryParse(reader.ReadVarString(), out ProductId);
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Id == Guid.Empty ? "" : Id.ToString());
            writer.WriteVarString(Type);
            writer.WriteVarString(PackId == Guid.Empty ? "" : PackId.ToString());
            writer.Write(IsDefault);
            writer.WriteVarString(ProductId == Guid.Empty ? "" : ProductId.ToString());
        }

        public static PersonaPieceData FromJson(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
            {
                Console.WriteLine("Is not JSON object at PersonaTintPieceData::FromJson");
                return Empty;
            }
            PersonaPieceData result = new();
            result.IsDefault = element.GetProperty("IsDefault").GetBoolean();
            Guid.TryParse(element.GetProperty("PackId").GetString() ?? "", out result.PackId);
            Guid.TryParse(element.GetProperty("PieceId").GetString() ?? "", out result.Id);
            result.Type = element.GetProperty("PieceId").GetString() ?? "";
            Guid.TryParse(element.GetProperty("ProductId").GetString() ?? "", out result.ProductId);
            return result;
        }
    }
}
