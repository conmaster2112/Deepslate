using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types.Skin
{
    public class SkinData : INetworkType
    {
        public string Id = string.Empty;
        public string PlayFabId = string.Empty;
        public string ResourcePatch = string.Empty;
        public ImageData SkinImage = ImageData.Empty;
        // Uint32 Length
        public AnimationData[] Animations = [];
        public ImageData CapeImage = ImageData.Empty;
        public string GeometryData = string.Empty;
        public string GeometryVersion = string.Empty;
        public string AnimationData = string.Empty;
        public string CapeId = string.Empty;
        public string FullId = string.Empty;
        public string ArmSize = string.Empty;
        public string SkinColor = string.Empty;
        // Uint32 Length
        public PersonaPieceData[] PersonaPieces = [];
        // Uint32 Length
        public PersonaTintPieceData[] PersonaTintPieces = [];

        public bool IsPremium = false;
        public bool IsPersona = false;
        public bool IsPersonaCapeOnClassic = false;
        public bool IsPrimaryUser = false;
        public bool IsOverridingPlayerAppearance = false;


        public void Read(ProtocolMemoryReader reader)
        {
            Id = reader.ReadVarString();
            PlayFabId = reader.ReadVarString(); 
            ResourcePatch = reader.ReadVarString();

            reader.Read(ref SkinImage);

            Animations = reader.ReadArray32<AnimationData>();

            reader.Read(ref CapeImage);
            GeometryData = reader.ReadVarString();
            GeometryVersion = reader.ReadVarString();
            AnimationData = reader.ReadVarString();
            CapeId = reader.ReadVarString();
            FullId = reader.ReadVarString();
            ArmSize = reader.ReadVarString();
            SkinColor = reader.ReadVarString();

            PersonaPieces = reader.ReadArray32<PersonaPieceData>();
            PersonaTintPieces = reader.ReadArray32<PersonaTintPieceData>();

            IsPremium = reader.ReadBool();
            IsPersona = reader.ReadBool();
            IsPersonaCapeOnClassic = reader.ReadBool();
            IsPrimaryUser = reader.ReadBool();
            IsOverridingPlayerAppearance = reader.ReadBool();
        }
        public void Write(ProtocolMemoryWriter writer)
        {
            writer.WriteVarString(Id);
            writer.WriteVarString(PlayFabId);
            writer.WriteVarString(ResourcePatch);
            writer.Write(SkinImage);

            writer.WriteArray32(Animations);

            writer.Write(CapeImage);

            writer.WriteVarString(GeometryData);
            writer.WriteVarString(GeometryVersion);
            writer.WriteVarString(AnimationData);
            writer.WriteVarString(CapeId);
            writer.WriteVarString(FullId);
            writer.WriteVarString(ArmSize);
            writer.WriteVarString(SkinColor);

            writer.WriteArray32(PersonaPieces);
            writer.WriteArray32(PersonaTintPieces);

            writer.Write(IsPremium);
            writer.Write(IsPersona);
            writer.Write(IsPersonaCapeOnClassic);
            writer.Write(IsPrimaryUser);
            writer.Write(IsOverridingPlayerAppearance);
        }
        public static SkinData FromJwtPayload(JwtPayload payload)
        {
            SkinData skin = new();
            foreach(var kv in payload)
            {
                switch (kv.Key)
                {
                    case "ArmSize":
                        skin.ArmSize = (string)kv.Value;
                        break;
                    case "CapeId":
                        skin.CapeId = (string)kv.Value;
                        break;
                    case "CapeData":
                        skin.CapeImage.Data64 = (string)kv.Value;
                        break;
                    case "CapeImageHeight":
                        skin.CapeImage.Height = (uint)(int)kv.Value;
                        break;
                    case "CapeImageWidth":
                        skin.CapeImage.Width = (uint)(int)kv.Value;
                        break;
                    case "CapeOnClassicSkin":
                        skin.IsPersonaCapeOnClassic = (bool)kv.Value;
                        break;
                    case "TrustedSkin":
                        skin.IsPrimaryUser = (bool)kv.Value;
                        break;
                    case "SkinResourcePatch":
                        skin.ResourcePatch = (string)kv.Value;
                        break;
                    case "SkinId":
                        skin.FullId = skin.Id = (string)kv.Value;
                        break;
                    case "SkinImageWidth":
                        skin.SkinImage.Width = (uint)(int)kv.Value;
                        break;
                    case "SkinImageHeight":
                        skin.SkinImage.Height = (uint)(int)kv.Value;
                        break;
                    case "SkinData":
                        skin.SkinImage.Data64 = (string)kv.Value;
                        break;
                    case "SkinGeometryData":
                        skin.GeometryData = (string)kv.Value;
                        break;
                    case "SkinGeometryDataEngineVersion":
                        skin.GeometryVersion = (string)kv.Value;
                        break;
                    case "SkinColor":
                        skin.SkinColor = (string)kv.Value;
                        break;
                    case "PersonaSkin":
                        skin.IsPersona = (bool)kv.Value;
                        break;
                    case "SkinAnimationData":
                        skin.AnimationData = (string)kv.Value;
                        break;
                    case "PlayFabId":
                        skin.PlayFabId = (string)kv.Value;
                        break;
                    case "PremiumSkin":
                        skin.IsPremium = (bool)kv.Value;
                        break;
                    case "OverrideSkin":
                        skin.IsOverridingPlayerAppearance = (bool)kv.Value;
                        break;
                    case "PieceTintColors":
                        JsonElement[] elemetTints = ((JsonElement)kv.Value).EnumerateArray().ToArray();
                        skin.PersonaTintPieces = new PersonaTintPieceData[elemetTints.Length];
                        for (int i = 0; i < elemetTints.Length; i++)
                        {
                            skin.PersonaTintPieces[i] = PersonaTintPieceData.FromJson(elemetTints[i]);
                        }
                        break;
                    case "PersonaPieces":
                        JsonElement[] elemetPieces = ((JsonElement)kv.Value).EnumerateArray().ToArray();
                        skin.PersonaPieces = new PersonaPieceData[elemetPieces.Length];
                        for (int i = 0; i < elemetPieces.Length; i++)
                        {
                            skin.PersonaPieces[i] = PersonaPieceData.FromJson(elemetPieces[i]);
                        }
                        break;
                    case "AnimatedImageData":
                        JsonElement[] elemetAnimations = ((JsonElement)kv.Value).EnumerateArray().ToArray();
                        skin.Animations = new AnimationData[elemetAnimations.Length];
                        for (int i = 0; i < elemetAnimations.Length; i++)
                        {
                            skin.Animations[i] = Skin.AnimationData.FromJson(elemetAnimations[i]);
                        }
                        break;
                    default:
                        break;
                }
            }
            return skin;
        }
    }
}
