using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Service;
using ConMaster.Deepslate.Types;


namespace ConMaster.Deepslate.Entities
{
    public class Player: Entity
    {
        internal Player(Gamer gamer): base(PlayerEntityType.Instance)
        {

        }

        //ChunkManager = new(this);
        /*_Abilities = new()
        {
            ActorUniqueId = UniqueId,
            CommandPermissionLevel = CommandPermissionLevel.Normal,
            PermissionLevel = PermissionLevel.Member,
            Layers = 
            [
                new()
                {
                    LayerType = AbilityLayerType.Base,
                    AbilitiesAllowed = AbilityFlag.All,
                    AbilitiesEnabled = 0,
                    FlySpeed = 0.1f,
                    WalkSpeed = 0.1f
                }    
            ],
        };
        Client = client;
        _ActorFlags = (ulong)(
            ActorFlags.HasGravity
            | ActorFlags.Breathing | ActorFlags.Moving 
            | ActorFlags.CanWalk | ActorFlags.CanClimb
            | ActorFlags.Breathing
            );*/

        /* public const int PLAYER_MASK_PLAYER_UPDATE_BITS_SIZE = 2;
         public const uint ABILITIES_UPDATE_BIT = 0b0001 << ENTITY_MASK_UPDATE_BITS_SIZE;
         public const uint GAMEMODE_PLAYER_UPDATE_BIT = 0b0001;
         public const uint PACKET_QUEUE_PLAYER_UPDATE_BIT = 0b0010;
         protected uint _PLAYER_UPDATE_BITS = 0;

         public PlayerChunkManager ChunkManager = null!;
         public void SetPlayerUpdateBits(uint updateBits) => Interlocked.Or(ref _PLAYER_UPDATE_BITS, updateBits);
         public static bool HasPlayerUpdateBits(uint mask, uint updateBits) => (mask & updateBits) == updateBits;
         protected PlayerAbilitiesData _Abilities;
         protected GameMode _GameMode;
         public void SetAbility(ulong a)
         {
             _ActorFlags = a;
             SetUpdateBit(UPDATE_ACTOR_FLAGS_BIT);
         }*/

        /*
        protected override void SetAllUpdateBits(uint updateBit)
        {
            updateBit |= ABILITIES_UPDATE_BIT;
            base.SetAllUpdateBits(updateBit);
        }
        protected ConcurrentQueue<IPacket> _packetsQueue = new();
        protected void SendPacket(IPacket packet)
        {
            _packetsQueue.Enqueue(packet);
            SetPlayerUpdateBits(PACKET_QUEUE_PLAYER_UPDATE_BIT);
        }




        ///
        /// PUBLIC APIS HERE
        ///
        public GameMode GameMode { get => _GameMode; set { _GameMode = value; SetPlayerUpdateBits(GAMEMODE_PLAYER_UPDATE_BIT); } }
        public ulong GuildId => Client.GuildId;
        //public string Name => Client.Name;
        public Client Client { get; private init; }
        public PermissionLevel PermissionLevel
        {
            get => _Abilities.PermissionLevel;
            set
            {
                _Abilities.PermissionLevel = value;
                SetUpdateBit(ABILITIES_UPDATE_BIT);
            }
        }
        public CommandPermissionLevel CommandPermissionLevel
        {
            get => _Abilities.CommandPermissionLevel;
            set
            {
                _Abilities.CommandPermissionLevel = value;
                SetUpdateBit(ABILITIES_UPDATE_BIT);
            }
        }
        public AbilityFlag Abilities
        {
            get => _Abilities.Layers[0].AbilitiesEnabled;
            set
            {
                _Abilities.Layers[0].AbilitiesEnabled = value;
                SetUpdateBit(ABILITIES_UPDATE_BIT);
            }
        }

        public void Disconnect(string reason)
        {
            DisconnectPacket disconnectPacket = new DisconnectPacket();
        }
        public void SendToast(string title, string content)
        {
            using ToastRequestPacket packet = ToastRequestPacket.Create();
            packet.Title = title;
            packet.Content = content;
            SendPacket(packet);
        }/*
        public void SendDebug(DebugMarkerOptions options)
        {
            ClientboundDebugRendererPacket packet = ClientboundDebugRendererPacket.Create();
            packet.Type = 2;
            packet.MarkerOptions = options;
            SendPacket(packet);
        }*/

        ///
        /// INTERNAL BEHAVIOR HERE
        ///
        /*
        // We dont have to update entity position for players
        protected override void EntityTick(ulong currentTick)
        {
            using NetworkChunkPublisherUpdatePacket p = NetworkChunkPublisherUpdatePacket.Create();
            p.ViewPosition = BlockLocation;
            p.NewRadiusOfView = ChunkManager.RenderDistance << 4;
            p.Vec2ChunkPositions = [];
            Client.SendPacket(p);
        }


        // When Player updates we have to send changes to all clients, including them self
        protected override IEnumerable<IPacket> SyncPackets(uint updateBits)
        {
            foreach (IPacket p in base.SyncPackets(updateBits)) yield return p; 
            if(HasUpdateBit(updateBits, ABILITIES_UPDATE_BIT))
            {
                using UpdateAbilitiesPacket packet = UpdateAbilitiesPacket.Create();
                packet.Data = _Abilities;
                yield return packet;
            }
        }


        // We can send GameMode changes or Chat only to this player
        protected virtual IEnumerable<IPacket> SelfSyncPackets(uint updateBits)
        {
            if (HasPlayerUpdateBits(updateBits, GAMEMODE_PLAYER_UPDATE_BIT))
            {
                using SetPlayerGameModePacket packet = SetPlayerGameModePacket.Create();
                packet.GameMode = _GameMode;
                yield return packet;
            }
            if(HasPlayerUpdateBits(updateBits, PACKET_QUEUE_PLAYER_UPDATE_BIT))
            {
                while(_packetsQueue.TryDequeue(out var packet))
                {
                    yield return packet;
                    if (packet is IDisposable dis) dis.Dispose(); 
                }
            }
        }

        protected override void Update()
        {
            uint bits = Interlocked.Exchange(ref _PLAYER_UPDATE_BITS, 0);
            if (bits != 0)
            {
                Client.SendPacket(SelfSyncPackets(bits));
            }
            base.Update();
        }

        internal virtual void OnPlayerMoved(MovePlayerPacket packet)
        {

            Vec3 loc = packet.Position;
            IsOnGround = packet.IsOnGround;
            Velocity = loc - Location;
            Location = loc;
            Rotation = packet.Rotation;
            if(IsSpawned) ChunkManager.OnPlayerMove(Vec2ChunkPosition);
        }*/
    }
}
