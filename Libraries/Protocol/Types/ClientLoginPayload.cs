using ConMaster.Deepslate.Protocol.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace ConMaster.Deepslate.Protocol.Types
{
    public struct ClientLoginPayload()
    {
        public string Name = string.Empty;
        public ulong Xuid = 0;
        public Guid Uuid = Guid.Empty;
        public bool SupportsClientSideGeneration = false;
        public bool TrustedSkin = false;
        public bool IsEditorMode = false;
        public bool ThirdPartyNameOnly = false;
        public string LanguageCode = string.Empty;
        public string DeviceModel = string.Empty;
        public string GameVersion = string.Empty;
        public string ThirdPartyName = string.Empty;
        public Guid DeviceId = Guid.Empty;
        public DeviceOS DeviceOS = 0;
        public ulong PlayFabId = 0;
        public static ClientLoginPayload FromJwtPayload(JwtPayload payload)
        {
            ClientLoginPayload result = new();
            foreach(var kv in payload)
            {
                switch (kv.Key)
                {
                    case "CompatibleWithClientSideChunkGen":
                        result.SupportsClientSideGeneration = (bool)kv.Value;
                        break;
                    case "DeviceId":
                        result.DeviceId = Guid.Parse((string)kv.Value);
                        break;
                    case "DeviceModel":
                        result.DeviceModel = (string)kv.Value;
                        break;
                    case "DeviceOS":
                        result.DeviceOS = (DeviceOS)kv.Value;
                        break;
                    case "GameVersion":
                        result.GameVersion = (string)kv.Value;
                        break;
                    case "IsEditorMode":
                        result.IsEditorMode = (bool)kv.Value;
                        break;
                    case "LanguageCode":
                        result.LanguageCode = (string)kv.Value;
                        break;
                    case "PlayFabId":
                        result.PlayFabId = ulong.Parse((string)kv.Value, System.Globalization.NumberStyles.HexNumber);
                        break;
                    case "ThirdPartyName":
                        result.ThirdPartyName = (string)kv.Value;
                        break;
                    case "ThirdPartyNameOnly":
                        result.ThirdPartyNameOnly = (bool)kv.Value;
                        break;
                    case "TrustedSkin":
                        result.TrustedSkin = (bool)kv.Value;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
