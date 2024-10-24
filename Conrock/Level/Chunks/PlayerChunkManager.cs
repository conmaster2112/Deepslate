using ConMaster.Bedrock.Base;
using ConMaster.Bedrock.Data;
using System.Collections.Concurrent;

namespace ConMaster.Bedrock.Level.Chunks
{
    public class PlayerChunkManager
    {
        public byte RenderDistance = 10;
        public byte MaxChunksPerTick = 4;
        public ChunkPosition LastChunkPosition { get; private set; } = new(int.MinValue, int.MinValue);

        public Player Player { get; private init; }
        private HashSet<ChunkPosition> LoadedChunks { get; init; } = new();
        public PriorityQueue<ChunkPosition> Requests { get; private init; } = new();
        public ConcurrentQueue<ChunkPosition> Releases { get; private init; } = new();
        public PlayerChunkManager(Player player)
        { 
            Player = player;
        }
        internal void OnPlayerMove(ChunkPosition position)
        {
            if(LastChunkPosition != position)
            {
                LastChunkPosition = position;
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
                ChunkPosition c = chunk - LastChunkPosition;
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
                    ChunkPosition p = new ChunkPosition(x, z) + LastChunkPosition;
                    if (LoadedChunks.Add(p))
                    {
                        Requests.Enqueue(p, priority);
                    }
                }
        }
    }
}
