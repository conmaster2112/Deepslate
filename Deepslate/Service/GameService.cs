using ConMaster.Deepslate.Entities;
using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol.Enums;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
namespace ConMaster.Deepslate.Service
{
    public partial class GameService
    {
        public event Action<object, string>? OnWarn;
        public event ErrorEventHandler? OnError;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsRunning { get; private set; } = false;
        private readonly HashSet<Game> _games = [];
        private readonly HashSet<Server> _servers = [];
        private readonly ConcurrentDictionary<Client, Gamer> _gamers = [];
        [MaybeNull]
        private Game _defaultGame = null!;
        public IReadOnlyCollection<Server> Servers => _servers;
        public IReadOnlyCollection<Game> Games => _games;
        public bool TrySetDefaultGame(Game game)
        {
            if (!_games.Contains(game)) return false;
            _defaultGame = game;
            return true;
        }
        public void AddServer(Server server)
        {
            if (!_servers.Add(server)) return;
            server.OnError += (s, v)=>OnError?.Invoke(s, v);
            server.OnWarn += (s, v)=>OnWarn?.Invoke(s,v);
            server.ClientConnected.When += (s, client) =>
            {
                if (_defaultGame == null)
                {
                    client.Client.Disconnect((int)DisconnectReason.SessionNotFound, "No game is running player doesn't have place to spawn in");
                    return;
                }
            };
            server.ClientDisconnected.When += (s, client) =>
            {
                if(_gamers.TryGetValue(client.Client, out var gamer))
                {
                    Console.WriteLine("Gamer Disconnected: " + gamer.Name);
                }
            };
        }
        public void AddGame(Game game)
        {
            _games.Add(game);
            _defaultGame ??= game;
        }
        public void Start()
        {
            lock (this)
            {
                if (IsRunning) return;
                IsRunning = true;
                CancellationToken token = CancellationToken.None;
                foreach (Server server in _servers) server.Start();
                foreach (Game game in _games) _ = game.Run(token);
            }
        }
        public void Stop()
        {
            lock (this)
            {
                foreach (Server server in _servers) server.Stop();
            }
        }
    }
}