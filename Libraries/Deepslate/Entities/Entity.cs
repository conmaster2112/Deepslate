using ConMaster.Deepslate.Worlds;
using ConMaster.Deepslate.Types;
using ConMaster.Deepslate.Protocol.Types;

namespace ConMaster.Deepslate.Entities
{
    public class Entity
    {
        private static ulong LastUniqueId = 0;

        /*
        public const int ENTITY_MASK_UPDATE_BITS_SIZE = 2;
        public const uint UPDATE_ATTRIBUES_BIT = 0b01;
        public const uint UPDATE_ACTOR_FLAGS_BIT = 0b10;
        protected volatile uint _updates = 0;*/

        private readonly Dictionary<int, object> _components = [];
        /*protected ulong _ActorFlags = (ulong)(ActorFlags.HasGravity | ActorFlags.Breathing);*/
        /*
        protected void SetUpdateBit(uint updateBit) => Interlocked.Or(ref _updates, updateBit);
        protected static bool HasUpdateBit(uint mask, uint updateBit) => (mask & updateBit) == updateBit;
        protected virtual void SetAllUpdateBits(uint updateBit)
        {
            updateBit |= UPDATE_ATTRIBUES_BIT | UPDATE_ACTOR_FLAGS_BIT;
            SetUpdateBit(updateBit);
        }
        */

        //private readonly HashSet<EntityAttributeComponent> _ChangedAttributes = [];
        //public ConcurrentQueue<IPacket> _updatePackets = new();
        /*public bool IsSpawned { get; protected set; }
        public bool IsOnGround { get; protected set; }
        public bool HasGravity
        {
            get => ((ActorFlags)_ActorFlags & ActorFlags.HasGravity) == ActorFlags.HasGravity;
            set
            {
                if (value) _ActorFlags |= (ulong)ActorFlags.HasGravity;
                else _ActorFlags &= ~(ulong)ActorFlags.HasGravity;
                SetUpdateBit(UPDATE_ACTOR_FLAGS_BIT);
            }
        }
        public void SetActorFlags(ActorFlags flags)
        {
            _ActorFlags = (ulong)flags;
            SetUpdateBit(UPDATE_ACTOR_FLAGS_BIT);
        }*/
        public Dimension? Dimension { get; protected set; }
        public readonly long UniqueId = (long)Interlocked.Increment(ref LastUniqueId);
        public readonly ulong RuntimeId;
        public readonly EntityType Type;
        public string TypeId => Type.Id;
        public Entity(EntityType type) {
            RuntimeId = (ulong)UniqueId;
            Velocity = Location = Vec3f.Zero;
            Type = type;
            //HasGravity = true;
            //SetAllUpdateBits(0);
        }
        public Vec2f Rotation { get; protected set; }
        public Vec3f Location { get; protected set; }
        public Vec3i BlockLocation
        {
            get
            {
                Vec3f loc = Location;
                return new((int)loc.X, (int)loc.Y, (int)loc.Z);
            }
        }
        public Vec2ChunkPosition Vec2ChunkPosition => new((int)MathF.Floor(Location.X) >> 4, (int)MathF.Floor(Location.Z) >> 4);
        public Vec3f Velocity { get; protected set; }

        //public EntityComponent GetComponent(string Id) => _Components[Id.GetHashCode()];
        //public bool TryGetComponent(string id, [MaybeNullWhen(false)] out EntityComponent component) => _Components.TryGetValue(id.GetHashCode(), out component);




















        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }
        /*
        protected virtual void EntityTick(ulong currentTick)
        {
            Location += Velocity;
            Velocity /= 0.5f;
            Velocity += Dimension.GravityForce;
        }
        public virtual void Tick(ulong currentTick)
        {
            Update();
            EntityTick(currentTick);
        }
        protected virtual void Update()
        {
            uint bits = Interlocked.Exchange(ref _updates, 0);
            if (bits != 0)
            {
                Dimension.Broadcast(SyncPackets(bits));
            }
        }
        /*protected virtual IEnumerable<IPacket> SyncPackets(uint updateBits)
        {
            if(HasUpdateBit(updateBits, UPDATE_ATTRIBUES_BIT))
            {
                using UpdateAttributesPacket packet = UpdateAttributesPacket.Create();
                packet.CurrentTick = Dimension.World.CurrentTick;
                packet.EntityRuntimeId = RuntimeId;
                List<IAttributeComponent> list = [];
                foreach (var att in _Components.Values)
                {
                    if(att is AttributeEntityComponent comp)
                    {
                        if (comp.HasChanged) list.Add(comp);
                    }
                }
                packet.AttributeList = list;
                yield return packet;
            }
            if(HasUpdateBit(updateBits, UPDATE_ACTOR_FLAGS_BIT))
            {
                using SetActorDataPacket packet = SetActorDataPacket.Create();
                packet.CurrentTick = Dimension.World.CurrentTick;
                packet.EntityRuntimeId = RuntimeId;
                packet.Items = [
                    new DataItem(){
                        DataUniqueId = 0, //Flags data Id, //flags_extended 92
                        Value = new LongDataItemValue(){Value = (long)_ActorFlags}
                    }
                ];
                yield return packet;
            }
        }*/

        /*

        internal static void SetDimensionFor(Entity entity, Dimension dimension) => entity.Dimension = dimension;
        internal static void SetIsSpawnedFor(Entity entity, bool spawned) => entity.IsSpawned = spawned;
        internal static void SetUpdateBitFor(Entity entity, uint updateBit) => entity.SetUpdateBit(updateBit);*/
    }
}
