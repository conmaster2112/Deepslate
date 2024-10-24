using ConMaster.Bedrock.Base;
using ConMaster.Bedrock.Data;
using ConMaster.Bedrock.Data.Types;
using ConMaster.Bedrock.Level.Chunks;
using ConMaster.Buffers;
using System.Collections.Concurrent;

namespace ConMaster.Bedrock.Level
{
    public class Dimension
    {
        internal readonly ConcurrentDictionary<long, Entity> _Entities = new();
        internal readonly ConcurrentDictionary<long, Player> _Players = new();
        public readonly string UniqueId;
        public readonly DimensionType Type;
        public readonly ChunkManager ChunkManager;
        public readonly World World;
        public Vec3 GravityForce = new(0, -0.1f, 0);
        public string TypeId => Type.Id;
        public int NetworkId => Type.NetworkId;
        public IEnumerable<Entity> Entities => _Entities.Values;
        public IEnumerable<Player> Players => _Players.Values;
        public Dimension(string uniqueId, DimensionType type, World world)
        {
            UniqueId = uniqueId;
            World = world;
            Type = type;
            ChunkManager = new(this);
        }
        internal void Tick(ulong currentTick)
        {
            foreach (var entity in Entities) entity.Tick(currentTick);
            ChunkManager.Tick(currentTick);
        }
        public void Broadcast(IEnumerable<IPacket> packets)
        {
            using RentedBuffer buffer = Network.Network.BuildPacketsRawPayload(packets, World.Game.Runner.Network.Deflate);
            foreach (Entity e in Entities)
            {
                if (e is Player p)
                {
                    p.Client.InternalSendRawPayload(buffer.Span);
                }
            }
        }
        public void Spawn(Entity entity)
        {
            Entity.SetDimensionFor(entity, this);
            _Entities[entity.UniqueId] = entity;
            if (entity is Player p) _Players[entity.UniqueId] = p;
            Entity.SetIsSpawnedFor(entity, true);
        }
        public void Remove(Entity entity)
        {
            if (_Entities.TryRemove(entity.UniqueId, out _))
            {
                Console.WriteLine("Removed Entity");
            }
            if (entity is Player) _Players.TryRemove(entity.UniqueId, out _);
        }
    }
}
