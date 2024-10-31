using ConMaster.Deepslate.Entities;
using ConMaster.Deepslate.Protocol.Types;
using System.Collections.Concurrent;

namespace ConMaster.Deepslate.Worlds.Chunks
{
    public class PlayerChunkManager
    {
        public byte RenderDistance = 10;
        public byte MaxChunksPerTick = 4;
        public Vec2ChunkPosition LastVec2ChunkPosition { get; private set; } = new(int.MinValue, int.MinValue);

        public Player Player { get; private init; }
        private HashSet<Vec2ChunkPosition> LoadedChunks { get; init; } = new();
        public PriorityQueue<Vec2ChunkPosition> Requests { get; private init; } = new();
        public ConcurrentQueue<Vec2ChunkPosition> Releases { get; private init; } = new();
        public PlayerChunkManager(Player player)
        { 
            Player = player;
        }
        internal void OnPlayerMove(Vec2ChunkPosition position)
        {
            if(LastVec2ChunkPosition != position)
            {
                LastVec2ChunkPosition = position;
                Update();
            }
        }
        private void Update()
        {
            var main = Player.Dimension.ChunkManager;
            int renderDist = RenderDistance;
            int distancePower = renderDist * renderDist;
            foreach (var chunk in LoadedChunks)
            {
                Vec2ChunkPosition c = chunk - LastVec2ChunkPosition;
                if (c.LengthPower > distancePower)
                {
                    LoadedChunks.Remove(chunk);
                    Releases.Enqueue(chunk);
                }
            }
            renderDist--;
            for (int x = -renderDist; x <= renderDist; x++)
                for (int z = -renderDist; z <= renderDist; z++)
                {
                    int priority = x * x + z * z;
                    if (priority > distancePower) continue;
                    Vec2ChunkPosition p = new Vec2ChunkPosition(x, z) + LastVec2ChunkPosition;
                    if (LoadedChunks.Add(p))
                    {
                        Requests.Enqueue(p, priority);
                    }
                }
        }
    }
}
