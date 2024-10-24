using ConMaster.Bedrock.Data;
using ConMaster.Bedrock.Enums;
using ConMaster.Bedrock.Network;
using ConMaster.Bedrock.Packets;
using ConMaster.Bedrock.Types;
using ConMaster.Bedrock.Types.Skin;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace ConMaster.Bedrock.Engine.Handlers
{
    internal static class LoginPacketHandler
    {
        public static readonly JwtSecurityTokenHandler JwtParser = new()
        {
            MaximumTokenSizeInBytes = int.MaxValue
        };

        //Using Worker Handler as it runs on different thread
        public static ProtocolPacketWorkerHandler<LoginPacket> GetHandler()
        {
            return new(Handler, LoginPacket.PACKET_ID);
        }
        public static void Handler(Client client, LoginPacket packet)
        {
            //Login Packet is released once this synced method returns so we save its value for later use
            ClientTokenData data = packet.Data;

            //Identity Data parse
            JsonElement obj = JsonDocument.Parse(data.IdentityData).RootElement;
            if (obj.ValueKind != JsonValueKind.Object)
            {
                client.Disconnect("Client Data Expected in JSON Object Format, {\"chain\":[...]}", DisconnectReason.LoginPacketNoCert);
                return;
            }


            JsonElement chains = obj.GetProperty("chain");
            if (chains.ValueKind != JsonValueKind.Array)
            {
                client.Disconnect("Client Data Expected in JSON Object Format, {\"chain\":[...]}", DisconnectReason.LoginPacketNoCert);
                return;
            }


            JsonElement? extraData = null;
            string identityPublicKey = string.Empty;
            foreach (JsonElement value in chains.EnumerateArray())
            {
                if (value.ValueKind != JsonValueKind.String) continue;
                JwtPayload inner_payload = ParseJWT(value.GetString()!);
                if (inner_payload.ContainsKey("extraData"))
                {
                    extraData = (JsonElement)inner_payload["extraData"];
                }
                if (inner_payload.ContainsKey("identityPublicKey"))
                {
                    object o = inner_payload["identityPublicKey"];
                    if (o is string s) identityPublicKey = s;
                }
            }

            if (!extraData.HasValue)
            {
                client.Disconnect("No extraData provided in Jwt payload", DisconnectReason.LoginPacketNoCert);
                return;
            }

            if (extraData.Value.ValueKind != JsonValueKind.Object)
            {
                client.Disconnect("Expected extraData as JSON Object payload", DisconnectReason.LoginPacketNoCert);
                return;
            }
            JsonElement element = extraData.Value;

            JwtPayload payload = ParseJWT(data.ClientData);
            ClientLoginPayload cData = ClientLoginPayload.FromJwtPayload(payload);

            foreach (var value in element.EnumerateObject())
            {
                if (value.Value.ValueKind != JsonValueKind.String)
                {
                    continue;
                }
                switch (value.Name)
                {
                    case "XUID":
                        cData.Xuid = ulong.Parse(value.Value.GetString()??"0");
                        break;
                    case "displayName":
                        cData.Name = value.Value.GetString()!;
                        break;
                    case "identity":
                        cData.Uuid = Guid.Parse(value.Value.GetString()!);
                        break;
                    default:
                        break;
                }
            }


            using PlayStatusPacket login = PlayStatusPacket.Create();
            using ResourcePacksInfoPacket info = ResourcePacksInfoPacket.Create();
            login.Status = PlayStatus.LoginSuccess;


            Client.SetLoginPayload(client, cData);
            Client.SetSkin(client, SkinData.FromJwtPayload(payload));
            Client.SetConnectionState(client, ConnectionState.Connected);

            Client.SetPlayer(client, new Player(client));

            client.SendPacket([login, info]);
        }
        public static JwtPayload ParseJWT(string data)
        {
            return JwtParser.ReadJwtToken(data).Payload;
        }
    }
}
