using ConMaster.Bedrock.Base;
using ConMaster.Bedrock.Data;
using ConMaster.Bedrock.Data.Types;
using ConMaster.Bedrock.Enums;
using ConMaster.Bedrock.Level;
using ConMaster.Bedrock.Network;
using ConMaster.Bedrock.Packets;
using ConMaster.Bedrock.Types;
using System.Numerics;

namespace ConMaster.Bedrock.Engine.Handlers
{
    internal partial class GeneralHandlers
    {
        public static void AddProtocolHandlers(Protocol protocol)
        {
            protocol.AddPacketHandlerRaw<PacketViolationWarningPacket>(PacketViolationWarningPacketHandler, PacketViolationWarningPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<ClientCacheStatusPacket>(ClientCacheStatusPacketHandler, ClientCacheStatusPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<LoadingScreenInfoPacket>(LoadingScreenInfoPacketHandler, LoadingScreenInfoPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<MovePlayerPacket>(MovePlayerPacketHandler, MovePlayerPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<SetLocalPlayerAsInitializedPacket>(SetLocalPlayerAsInitializedPacketHandler, SetLocalPlayerAsInitializedPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<CommandRequestPacket>(CommandRequestPacketHandler, CommandRequestPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<PlayerActionPacket>(PlayerActionPacketHandler, PlayerActionPacket.PACKET_ID);
            protocol.AddPacketHandlerRaw<RequestChunkRadiusPacket>(RequestChunkRadiusPacketHandler, RequestChunkRadiusPacket.PACKET_ID);
        }
        public static void PacketViolationWarningPacketHandler(Client client, PacketViolationWarningPacket packet)
        {
            client.Server.Protocol.Log.Warn(packet.ViolationContext);
        }
        public static void ClientCacheStatusPacketHandler(Client client, ClientCacheStatusPacket packet)
        {
            client.Log.Info("Supports cache?: " + packet.SupportsCache);
        }
        public static void LoadingScreenInfoPacketHandler(Client client, LoadingScreenInfoPacket packet)
        {
            client.Log.Info("Loading screen numer: " + packet.LoadingStateId);
        }
        public static void MovePlayerPacketHandler(Client client, MovePlayerPacket packet)
        {
			client.Player.OnPlayerMoved(packet);
		}
        public static void SetLocalPlayerAsInitializedPacketHandler(Client client, SetLocalPlayerAsInitializedPacket packet)
        {
            var player = client.Player;
            client.Log.Success("Player Successfully Initialized");
            player.PermissionLevel = PermissionLevel.Member;
            player.CommandPermissionLevel = CommandPermissionLevel.Normal;
            player.Abilities |= (AbilityFlag.Build | AbilityFlag.Mine | AbilityFlag.DoorsAndSwitches | AbilityFlag.OpenContainers | AbilityFlag.AttackPlayers | AbilityFlag.AttackMobs);
			player.GameMode = GameMode.Creative; player.Abilities |= AbilityFlag.MayFly | AbilityFlag.Flying;

            //await Task.Delay(100);
            //player.ChunkManager.Send();
        }
        public static void CommandRequestPacketHandler(Client client, CommandRequestPacket packet)
        {
            client.Log.Info("Executed Command: " + packet.Command);
            client.Player.GameMode = GameMode.Creative;
            //client.Player.SendToast("§aINFO","Changed Gamemode to " + client.Player.GameMode);
            client.Player.Abilities |= AbilityFlag.MayFly;
			for(int i = 0; i < 65; i++)
			{
				/*await Task.Delay(3100);
				Console.WriteLine("New Ability: " + (ActorFlags)(1uL<<i), "Id: " + i);
				client.Player.SetActorFlags((ActorFlags)(1Lu << i));*/
			}
			client.Player.SendToast("Test", "Yes");
        }
        public static void PlayerActionPacketHandler(Client client, PlayerActionPacket packet)
        {
            client.Log.Info("Action " + packet.ActionId + " at " + packet.BlockPosition);
        }
        public static void RequestChunkRadiusPacketHandler(Client client, RequestChunkRadiusPacket packet)
        {
			using UpdateChunkRadiusPacket up = UpdateChunkRadiusPacket.Create();
			up.ChunkRadius = packet.ChunkRadius;
			client.Player.ChunkManager.RenderDistance = (byte)packet.ChunkRadius;
			client.SendPacket(up);
        }



        public static async void JoinProccess(Client client)
        {
            await Task.Yield();

            Dimension? dimension = client.Server.Runner.Game.World.GetDimension("custom:overworld");
			Player player = client.Player;
            if (dimension == null)
            {
                player.Disconnect("No dimension to join. . .");
                return;
            }

            LevelSettings settings = new()
            {
                 SpawnSettings = new() {
                     Type = 0,
                    BiomeName = "plains",
                    DimensionNetwrokId = DimensionTypes.Overworld.NetworkId
                 },
                 DefaultGameMode = GameMode.Survival,
                 AchievementsDisabled = true,
                 GeneratorType = 1,
                 DayCycleStopTime = 0,
                 AreEducationFeaturesEnabled = true,
                 WasMultiplayerIntended = true,
                 WasLanBroadcastIntended = true,
                 XboxBroadcastSettings = 0,
                 BroadcastSettings = 0,
                 CommandsEnabled = true,
                 GameRules = [
					new (){
						Name = "commandblockoutput",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "dodaylightcycle",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "doentitydrops",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "dofiretick",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "recipesunlock",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "dolimitedcrafting",
						IsEditable = true,
						Type = 1,
						Value = 0
					},
					new (){
						Name = "domobloot",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "domobspawning",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "dotiledrops",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "doweathercycle",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "drowningdamage",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "falldamage",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "firedamage",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "keepinventory",
						IsEditable = true,
						Type = 1,
						Value = 0
					},
					new (){
						Name = "mobgriefing",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "pvp",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "showcoordinates",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "naturalregeneration",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "tntexplodes",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "sendcommandfeedback",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "maxcommandchainlength",
						IsEditable = true,
						Type = 2,
						Value = 65_535
					},
					new (){
						Name = "doinsomnia",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "commandblocksenabled",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "randomtickspeed",
						IsEditable = true,
						Type = 2,
						Value = 1
					},
					new (){
						Name = "doimmediaterespawn",
						IsEditable = true,
						Type = 1,
						Value = 0
					},
					new (){
						Name = "showdeathmessages",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "functioncommandlimit",
						IsEditable = true,
						Type = 2,
						Value = 10_000
					},
					new (){
						Name = "spawnradius",
						IsEditable = true,
						Type = 2,
						Value = 10
					},
					new (){
						Name = "showtags",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "freezedamage",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "respawnblocksexplode",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "showbordereffect",
						IsEditable = true,
						Type = 1,
						Value = 1
					},
					new (){
						Name = "playerssleepingpercentage",
						IsEditable = true,
						Type = 2,
						Value = 100
					}
                 ],
                 Experiments = new()
                 {
                     WerePrevioslyEnabled = false,
                     Experiments = []
                 },
                 GameVersion = "1.21.22",
                 LimitedWorldDepth = 16,
                 LimitedWorldWidth = 16,
            };
            using StartGamePacket packet = StartGamePacket.Create();
            packet.LevelSettings = settings;
			packet.SpawnPosition = new Vec3(0, 35, 0);
            packet.UniqueId = player.UniqueId;
            packet.RuntimeId = player.RuntimeId;
            packet.GameMode = player.GameMode;
            packet.MultiplayerCorrelationId = "<raknet>a555-7ece-2f1c-8f69";
            packet.ServerVersion = "*";
            packet.LevelName = "Test Name";
            packet.PlayerMovementSettings.ServerAuthorityBlockBreaking = true;
			packet.PlayerMovementSettings.AuthorityMode = 0;
			packet.PlayerMovementSettings.RewindHistorySize = 0;
            packet.NetworkPermission.ServerAuthSoundEnabled = true;
            packet.BlockNetworkIdsAreHashed = true;
            packet.TemplateContentIdentity = Guid.Empty.ToString();
            packet.EnableItemStackManagement = true;

            using PlayStatusPacket p = PlayStatusPacket.Create();
            p.Status = PlayStatus.PlayerSpawn;
            client.SendPacket(packet);
            dimension.Spawn(player);
			await Task.Delay(100);
			client.SendPacket(p);
        }
    }
}
