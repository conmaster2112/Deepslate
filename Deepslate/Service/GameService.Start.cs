using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Packets;
using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Types;
namespace ConMaster.Deepslate.Service
{
    public partial class GameService
    {
        protected async void StartGameSequence(Gamer gamer)
        {
            LevelSettings settings = new()
            {
                SpawnSettings = new()
                {
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
            packet.SpawnPosition = new Vec3f(0, 35, 0);
            packet.UniqueId = 1;
            packet.RuntimeId = 1;
            packet.GameMode = GameMode.Creative;
            packet.MultiplayerCorrelationId = "<raknet>a555-7ece-2f1c-8f69";
            packet.ServerVersion = "*";
            packet.LevelName = "Test Name";
            packet.PlayerMovementSettings.ServerAuthorityBlockBreaking = true;
            packet.PlayerMovementSettings.AuthorityMode = 0;
            packet.PlayerMovementSettings.RewindHistorySize = 0;
            packet.NetworkPermission.ServerAuthSoundEnabled = true;
            packet.PlayerMovementSettings.AuthorityMode = 1;
            packet.BlockNetworkIdsAreHashed = true;
            packet.TemplateContentIdentity = Guid.Empty.ToString();
            packet.EnableItemStackManagement = true;


            gamer.Client.SendPacket([packet]);
            Console.WriteLine("Gamer Start Game Packet Sended");
            //dimension.Spawn(player);
            gamer.PlayStatus = PlayStatus.PlayerSpawn;
        }
    }
}
