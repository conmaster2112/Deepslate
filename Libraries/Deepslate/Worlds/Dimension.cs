
using ConMaster.Deepslate.Worlds.Chunks;
using System.Collections.Concurrent;
using ConMaster.Deepslate.Types;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Types;
using ConMaster.Deepslate.Entities;

namespace ConMaster.Deepslate.Worlds
{
    public class Dimension
    {
        protected readonly ConcurrentDictionary<long, Entity> _Entities = new();
        protected readonly ConcurrentDictionary<long, Player> _Players = new();
        public readonly string UniqueId;
        public readonly DimensionType Type;
        public readonly ChunkManager ChunkManager;
        public readonly World World;
        public Vec3f GravityForce = new(0, -0.1f, 0);
        public string TypeId => Type.Id;
        public int NetworkId => Type.NetworkId;
        public IReadOnlyCollection<Entity> Entities => (IReadOnlyCollection<Entity>)_Entities.Values;
        public IReadOnlyCollection<Player> Players => (IReadOnlyCollection<Player>)_Players.Values;
        public Dimension(string uniqueId, DimensionType type, World world)
        {
            UniqueId = uniqueId;
            World = world;
            Type = type;
            ChunkManager = new(this);
        }
        protected virtual void Tick(ulong currentTick)
        {
            //foreach (var entity in Entities) entity.Tick(currentTick);
            ChunkManager.Tick(currentTick);
        }
        public void Broadcast(IEnumerable<IPacket> packets, IEnumerable<Player>? players = default)
        {
            /*using RentedBuffer buffer = Network.Network.BuildPacketsRawPayload(packets, World.Game.Runner.Network.Deflate);
            foreach (Entity e in Entities)
            {
                if (e is Player p)
                {
                    p.Client.InternalSendRawPayload(buffer.Span);
                }
            }*/
        }
        /*
        public void Spawn(Entity entity)
        {
            //Entity.SetDimensionFor(entity, this);
            _Entities[entity.UniqueId] = entity;
            if (entity is Player p) _Players[entity.UniqueId] = p;
            //Entity.SetIsSpawnedFor(entity, true);
        }
        public void Remove(Entity entity)
        {
            if (_Entities.TryRemove(entity.UniqueId, out _))
            {
                Console.WriteLine("Removed Entity");
            }
            if (entity is Player) _Players.TryRemove(entity.UniqueId, out _);
        }*/
        public static void RunTick(Dimension dimension, ulong currentTick) => dimension.Tick(currentTick);
    }
}
