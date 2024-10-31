using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Network;

namespace ConMaster.Deepslate.Protocol.Types
{
    public class LevelSettings: INetworkType, ICloneable
    {
        public ulong Seed = 0;
        public SpawnSettings SpawnSettings = new() {
            BiomeName = "plains",
            Type = 0,
            DimensionNetwrokId = 0
        };
        public int GeneratorType;
        public GameMode DefaultGameMode;
        public bool IsHardcore;
        public Difficulty Difficulty;
        public  Vec3i  DefaultSpawnPosition;
        public bool AchievementsDisabled;
        public int EditorWorldType;
        public bool IsCreatedInEditor;
        public bool IsExportedFromEditor;
        public int DayCycleStopTime;
        public int EducationEditionOffer;
        public bool AreEducationFeaturesEnabled;
        public string EducationProductId = string.Empty;
        public float RainLevel;
        public float LightningLevel;
        public bool HasPlatformLockedContent;
        public bool WasMultiplayerIntended;
        public bool WasLanBroadcastIntended;
        public int XboxBroadcastSettings;
        public int BroadcastSettings;
        public bool CommandsEnabled;
        public bool TexturePackRequired;
        public GameRule[] GameRules = [];
        public ExperimentsList Experiments;

        public bool HasBonusChestEnabled;
        public bool StartMapEnabled;

        public int PlayerDefaultPermission;
        public int ServerChunkTickRange;
        public bool HasLockedBehaviorPack;
        public bool HasLockedResourcePack;
        public bool IsFromLockedTemplate;
        public bool UseMsaGamerTagsOnly;
        public bool WasCreatedFromTemplate;
        public bool IsTemplateWithLockedSettings;
        public bool SpawnOnlyV1Villagers;
        public bool IsPersonaDisabled;
        public bool IsCustomSkinEnabled;
        public bool IsEmoteChatMuted;
        public string GameVersion = string.Empty;
        public int LimitedWorldWidth;
        public int LimitedWorldDepth;
        public bool IsNewNether;
        public EduSharedUriResourceType EduUriResource;
        public bool OverrideExperimentalGameplay;
        public byte ChatRestriction;
        public bool DisabledPlayerInteractions;
        public string ServerId = string.Empty;
        public string WroldIdFromServer = string.Empty;
        public string ScenarioIdFromServer = string.Empty;

        public LevelSettings Clone()
        {
            return new()
            {
                Seed = this.Seed,
                AchievementsDisabled = this.AchievementsDisabled,
                AreEducationFeaturesEnabled = this.AreEducationFeaturesEnabled,
                BroadcastSettings = this.BroadcastSettings,
                ChatRestriction = this.ChatRestriction,
                CommandsEnabled = this.CommandsEnabled,
                DayCycleStopTime = this.DayCycleStopTime,
                DefaultGameMode = this.DefaultGameMode,
                DefaultSpawnPosition = this.DefaultSpawnPosition,
                Difficulty = this.Difficulty,
                DisabledPlayerInteractions = this.DisabledPlayerInteractions,
                GameVersion = this.GameVersion,
                EditorWorldType = this.EditorWorldType,
                EducationEditionOffer = this.EducationEditionOffer,
                EducationProductId = this.EducationProductId,
                EduUriResource = this.EduUriResource,
                Experiments = this.Experiments,
                GameRules = this.GameRules,
                GeneratorType = this.GeneratorType,
                HasBonusChestEnabled = this.HasBonusChestEnabled,
                HasLockedBehaviorPack = this.HasLockedBehaviorPack,
                HasLockedResourcePack = this.HasLockedResourcePack,
                HasPlatformLockedContent = this.HasPlatformLockedContent,
                IsCreatedInEditor = this.IsCreatedInEditor,
                IsCustomSkinEnabled = this.IsCustomSkinEnabled,
                IsEmoteChatMuted = this.IsEmoteChatMuted,
                IsExportedFromEditor = this.IsExportedFromEditor,
                IsFromLockedTemplate = this.IsFromLockedTemplate,
                IsHardcore = this.IsHardcore,
                IsNewNether = this.IsNewNether,
                IsPersonaDisabled = this.IsPersonaDisabled,
                IsTemplateWithLockedSettings = this.IsTemplateWithLockedSettings,
                LightningLevel = this.LightningLevel,
                LimitedWorldDepth = this.LimitedWorldDepth,
                LimitedWorldWidth = this.LimitedWorldWidth,
                OverrideExperimentalGameplay = this.OverrideExperimentalGameplay,
                PlayerDefaultPermission = this.PlayerDefaultPermission,
                RainLevel = this.RainLevel,
                ScenarioIdFromServer = this.ScenarioIdFromServer,
                ServerChunkTickRange = this.ServerChunkTickRange,
                ServerId = this.ServerId,
                SpawnOnlyV1Villagers = this.SpawnOnlyV1Villagers,
                SpawnSettings = this.SpawnSettings,
                StartMapEnabled = this.StartMapEnabled,
                TexturePackRequired = this.TexturePackRequired,
                UseMsaGamerTagsOnly = this.UseMsaGamerTagsOnly,
                WasCreatedFromTemplate = this.WasCreatedFromTemplate,
                WasLanBroadcastIntended = this.WasLanBroadcastIntended,
                WasMultiplayerIntended = this.WasMultiplayerIntended,
                WroldIdFromServer = this.WroldIdFromServer,
                XboxBroadcastSettings = this.XboxBroadcastSettings,
            };
        }
        object ICloneable.Clone() => Clone();

        public void Read(ProtocolMemoryReader reader)
        {
            Seed = reader.ReadUInt64();
            reader.Read(ref SpawnSettings);
            GeneratorType = reader.ReadSignedVarInt();
            DefaultGameMode = NetworkEnums.ReadGameMode(reader);
            IsHardcore = reader.ReadBool();
            Difficulty = NetworkEnums.ReadDifficulty(reader);
            reader.Read(ref DefaultSpawnPosition);
            AchievementsDisabled = reader.ReadBool();
            EditorWorldType = reader.ReadSignedVarInt();
            IsCreatedInEditor = reader.ReadBool();
            IsExportedFromEditor = reader.ReadBool();
            DayCycleStopTime = reader.ReadSignedVarInt();
            EducationEditionOffer = reader.ReadSignedVarInt();
            AreEducationFeaturesEnabled = reader.ReadBool();
            EducationProductId = reader.ReadVarString();
            
            RainLevel = reader.ReadFloat();
            LightningLevel = reader.ReadFloat();
            
            HasPlatformLockedContent = reader.ReadBool();
            WasMultiplayerIntended = reader.ReadBool();
            WasLanBroadcastIntended = reader.ReadBool();
            
            XboxBroadcastSettings = reader.ReadSignedVarInt();
            BroadcastSettings = reader.ReadSignedVarInt();
            
            CommandsEnabled = reader.ReadBool();
            TexturePackRequired = reader.ReadBool();

            GameRules = reader.ReadVarArray<GameRule>();
            reader.Read(ref Experiments);
            
            HasBonusChestEnabled = reader.ReadBool();
            StartMapEnabled = reader.ReadBool();
            
            PlayerDefaultPermission = reader.ReadSignedVarInt();
            
            ServerChunkTickRange = reader.ReadInt32();
            
            HasLockedBehaviorPack = reader.ReadBool();
            HasLockedResourcePack = reader.ReadBool();
            IsFromLockedTemplate = reader.ReadBool();
            UseMsaGamerTagsOnly = reader.ReadBool();
            WasCreatedFromTemplate = reader.ReadBool();
            IsTemplateWithLockedSettings = reader.ReadBool();
            SpawnOnlyV1Villagers = reader.ReadBool();
            IsPersonaDisabled = reader.ReadBool();
            IsCustomSkinEnabled = reader.ReadBool();
            IsEmoteChatMuted = reader.ReadBool();
            
            GameVersion = reader.ReadVarString();
            
            LimitedWorldWidth = reader.ReadInt32();
            LimitedWorldDepth = reader.ReadInt32();

            IsNewNether = reader.ReadBool();

            reader.Read(ref EduUriResource);

            OverrideExperimentalGameplay = reader.ReadBool();
            ChatRestriction = reader.ReadUInt8();
            DisabledPlayerInteractions = reader.ReadBool();

            ServerId = reader.ReadVarString();
            WroldIdFromServer = reader.ReadVarString();
            ScenarioIdFromServer = reader.ReadVarString();
        }

        public void Write(ProtocolMemoryWriter writer)
        {

            writer.Write(Seed);
            writer.Write(SpawnSettings);
            
            writer.WriteSignedVarInt(GeneratorType);
            NetworkEnums.Write(writer, DefaultGameMode);
            writer.Write(IsHardcore);
            NetworkEnums.Write(writer, Difficulty);
            
            writer.Write(DefaultSpawnPosition);

            writer.Write(AchievementsDisabled);
            writer.WriteSignedVarInt(EditorWorldType);
            writer.Write(IsCreatedInEditor);
            writer.Write(IsExportedFromEditor);
            writer.WriteSignedVarInt(DayCycleStopTime);
            writer.WriteSignedVarInt(EducationEditionOffer);
            writer.Write(AreEducationFeaturesEnabled);
            writer.WriteVarString(EducationProductId);

            writer.Write(RainLevel);
            writer.Write(LightningLevel);

            writer.Write(HasPlatformLockedContent);
            writer.Write(WasMultiplayerIntended);
            writer.Write(WasLanBroadcastIntended);

            writer.WriteSignedVarInt(XboxBroadcastSettings);
            writer.WriteSignedVarInt(BroadcastSettings);

            writer.Write(CommandsEnabled);
            writer.Write(TexturePackRequired);

            writer.WriteVarArray(GameRules);
            writer.Write(Experiments);

            writer.Write(HasBonusChestEnabled);
            writer.Write(StartMapEnabled);
            writer.WriteSignedVarInt(PlayerDefaultPermission);
            writer.Write(ServerChunkTickRange);

            writer.Write(HasLockedBehaviorPack);
            writer.Write(HasLockedResourcePack);
            writer.Write(IsFromLockedTemplate);
            writer.Write(UseMsaGamerTagsOnly);
            writer.Write(WasCreatedFromTemplate);
            writer.Write(IsTemplateWithLockedSettings);
            writer.Write(SpawnOnlyV1Villagers);
            writer.Write(IsPersonaDisabled);
            writer.Write(IsCustomSkinEnabled);
            writer.Write(IsEmoteChatMuted);

            writer.WriteVarString(GameVersion);

            writer.Write(LimitedWorldWidth);
            writer.Write(LimitedWorldDepth);

            writer.Write(IsNewNether);

            writer.Write(EduUriResource);

            writer.Write(OverrideExperimentalGameplay);
            writer.Write(ChatRestriction);
            writer.Write(DisabledPlayerInteractions);

            writer.WriteVarString(ServerId);
            writer.WriteVarString(WroldIdFromServer);
            writer.WriteVarString(ScenarioIdFromServer);
        }
    }
}
