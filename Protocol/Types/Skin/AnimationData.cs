using System.Text.Json;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.Skin
{
    public struct AnimationData() : INetworkType
    {
        public static readonly AnimationData Empty = new();
        public ImageData Image = ImageData.Empty;
        public uint Type = 0;
        public float Frames = 0;
        public uint Expression = 0;
        public void Read(ProtocolMemoryReader reader)
        {
            reader.Read(ref Image);
            Type = reader.ReadUInt32();
            Frames = reader.ReadFloat();
            Expression = reader.ReadUInt32();
        }

        public readonly void Write(ProtocolMemoryWriter writer)
        {
            writer.Write(Image);
            writer.Write(Type);
            writer.Write(Frames);
            writer.Write(Expression);
        }

        public static AnimationData FromJson(JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
            {
                return Empty;
            }
            AnimationData result = new();
            result.Type = element.GetProperty("Type").GetUInt32();
            result.Frames = element.GetProperty("Frames").GetSingle();
            result.Expression = element.GetProperty("AnimationExpression").GetUInt32();
            ImageData image = result.Image;
            image.Data64 = element.GetProperty("Image").GetString() ?? "";
            image.Height = element.GetProperty("ImageHeight").GetUInt32();
            image.Width = element.GetProperty("ImageWidth").GetUInt32();
            result.Image = image;
            return result;
        }
    }
}
