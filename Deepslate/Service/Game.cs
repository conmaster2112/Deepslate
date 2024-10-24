using System.Collections.Concurrent;
using System.Diagnostics;
using ConMaster.Deepslate.Entities;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Worlds;

namespace ConMaster.Deepslate.Service
{
    public partial class Game
    {
        private ConcurrentDictionary<Client, Player> _players = [];
        public Game(GameService service, World world)
        {
            Service = service;
            World = world;
            World._SetGameFor(World, this);
            System = new(this);
        }
        public GameService Service { get; protected init; }
        public World World { get; protected init; }
        public GameSystem System { get; protected init; }
    }
}