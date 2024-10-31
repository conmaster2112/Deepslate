using ConMaster.Deepslate.Protocol.Types;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using static ConMaster.Deepslate.Service.Authentication;
using ConMaster.Deepslate.Protocol.Types.Skin;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Packets;

namespace ConMaster.Deepslate.Service
{
    public class PlayerLoginEventArgs(ClientChainExtraData data, Client client) : EventArgs()
    {
        public ClientChainExtraData Data { get; private init; } = data;
        public Client Client { get; private init; } = client;
        public bool Cancel { get; set; } = false;
    }
    public static class Authentication
    {
        private static ECDsaSecurityKey BuildECDsaSecurityKey(ReadOnlySpan<byte> key)
        {
            ECDsa ecdsa = ECDsa.Create();
            ecdsa.ImportSubjectPublicKeyInfo(key, out _);
            return new(ecdsa);
        }
        private static readonly byte[] _pubAuthKey = Convert.FromBase64String("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAECRXueJeTDqNRRgJi/vlRufByu/2G0i2Ebt6YMar5QX/R0DIIyrJMcUpruK4QveTfJSTp3Shlq4Gk34cD/4GUWwkv0DVuzeuB+tXija7HBxii03NHDbPAD0AKnLr2wdAp");
        public static ReadOnlySpan<byte> PublicAuthKey => _pubAuthKey;
        public static readonly JwtSecurityTokenHandler JwtSecurity = new()
        {
            MaximumTokenSizeInBytes = int.MaxValue
        };
        public static readonly TokenValidationParameters JwtValidationParams = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = BuildECDsaSecurityKey(PublicAuthKey),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = false
        };
        public struct ClientChainExtraData
        {
            [JsonPropertyName("identity")]
            public Guid Identity { get; set; }
            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }
            [JsonPropertyName("XUID")]
            public string XUID { get; set; }
            [JsonPropertyName("titleId")]
            public string? TitleId { get; set; }
            [JsonPropertyName("sandboxId")]
            public string? SandboxId { get; set; }
        }
        public struct ChainKeyIndex
        {
            [JsonPropertyName("extraData")]
            public ClientChainExtraData? ExtraData { get; set; }
            [JsonPropertyName("identityPublicKey")]
            [JsonConverter(typeof(JsonBase64BufferConverter))]
            public byte[] IdentityPublicKey { get; set; }
            [JsonPropertyName("certificateAuthority")]
            public bool CertificateAuthority { get; set; }
        }
        public static Memory<byte> GetJwtPayloadInPlace(ReadOnlySpan<byte> buffer)
        {
            int fDot = buffer.IndexOf((byte)'.');
            int sDot = buffer.Slice(fDot + 1).LastIndexOf((byte)'.');

            // Calculate the total size needed, including padding
            int totalSize = (sDot + 3) / 4 * 4;
            byte[] data = GC.AllocateUninitializedArray<byte>(totalSize);

            //Making sure that last bytes would be the padding character
            data[^3] = data[^2] = data[^1] = (byte)'=';

            //Copy the slice to that array
            buffer.Slice(fDot + 1, sDot).CopyTo(data);

            //Endocde and return
            Base64.DecodeFromUtf8InPlace(data, out int bytesWritten);
            return data.AsMemory(0, bytesWritten);
        }
        public static void ValidateToken(string token, ReadOnlySpan<byte> key = default)
        {
            if (key == default) key = PublicAuthKey;
            JwtValidationParams.IssuerSigningKey = BuildECDsaSecurityKey(key);
            JwtSecurity.ValidateToken(token, JwtValidationParams, out _);
        }
    }
    public partial class GameService
    {
        public event EventHandler<PlayerLoginEventArgs>? PlayerLogin;
        protected async void PlayerLogin_OnRecieved(BaseProtocol protocol, Client client, LoginPacket packet)
        {
            var tokenData = packet.Data;

            // Yield so the netwrok thread is able to conitinue on receiving packets
            // Be sure you are no longer using LoginPacket(packet) instance after yield, 
            // bc login packet gets already disposed once you yould the code executiin
            await Task.Yield();

            var info = LoginAuthentication(tokenData);


            PlayerLoginEventArgs args = new(info, client);

            PlayerLogin?.Invoke(this, args);

            if (args.Cancel)
            {
                client.Connection.Disconnect();
                return;
            }


            if (_defaultGame == null)
            {
                client.Disconnect((int)DisconnectReason.SessionNotFound);
                return;
            }

            Gamer gamer = new(_defaultGame, client, info);
            _gamers[client] = gamer;



            // We should create a player instance here
            // Also i am not sure about having a seperated player from client, bc not sure how fast the hash map is
            // So not sure if there isn't better way to link client to player
            // Also GameService is really abstract and too general, so spawning player in specific word would be kinda complex,
            // for example each packet would have to be redirected to specific world and more if you know what i mean
            Console.WriteLine(gamer.Name);
            Console.WriteLine(gamer.Xuid);
            Console.WriteLine(gamer.IsXboxSigned);
            using var packs = BuildResourcePackInfoPacket(gamer);
            using PlayStatusPacket status = PlayStatusPacket.FromStatus(PlayStatus.LoginSuccess);
            client.SendPacket([status, packs]);
        }
        protected ClientChainExtraData LoginAuthentication(ClientTokenData tokenData)
        {
            Span<byte> identityData = tokenData.IdentityDataUtf8;
            Utf8JsonReader reader = new(identityData);
            bool isFirst = true;
            ChainKeyIndex lastChainIndex = default;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.String && reader.CurrentDepth == 2)
                {
                    //Get raw data
                    ReadOnlySpan<byte> rawUtf8Bytes = reader.ValueSpan;

                    Span<byte> _jwtPayload = GetJwtPayloadInPlace(rawUtf8Bytes).Span;

                    ChainKeyIndex current = JsonSerializer.Deserialize<ChainKeyIndex>(_jwtPayload);

                    if (isFirst)
                    {
                        if (current.ExtraData == null)
                        {
                            isFirst = false;
                            continue;
                        }
                        else
                        {
                            //Deserialize JWT Payload
                            lastChainIndex = current;
                            break;
                        }
                    }

                    //Validate JWT by validation token
                    // We can use ASCII bc we expect the JWT is valid JWT and every WTL is made from valid ASCII characters
                    ValidateToken(Encoding.ASCII.GetString(rawUtf8Bytes), lastChainIndex.IdentityPublicKey ?? PublicAuthKey);

                    //Deserialize JWT Payload
                    lastChainIndex = current;
                }
            }

            Span<byte> clientData = tokenData.ClientDataUtf8;

            ValidateToken(Encoding.ASCII.GetString(clientData), lastChainIndex.IdentityPublicKey);

            Span<byte> jwtPayload = GetJwtPayloadInPlace(clientData).Span;

            if (lastChainIndex.ExtraData == null) throw new SecurityTokenException("No extradata in last JWT chain");

            return (ClientChainExtraData)lastChainIndex.ExtraData;
        }
    }
}
