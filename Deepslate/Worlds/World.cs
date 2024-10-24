using ConMaster.Deepslate.Entities;
using ConMaster.Deepslate.Service;

namespace ConMaster.Deepslate.Worlds
{
    public class World
    {
        public string LevelName { get; set; } = string.Empty;
        public Game Game { get; protected set; } = null!;
        public Dimension? DefaultDimension { get; protected set; }
        protected Dictionary<string, Dimension> _dimensions = [];
        public ulong CurrentTick => Game?.CurrentTick??0;
        public IReadOnlyCollection<Dimension> Dimensions => _dimensions.Values;
        public virtual Dimension? GetDimension(string id) { _dimensions.TryGetValue(id, out Dimension? dimension); return dimension; }
        public virtual void AddDimension(Dimension dimension)
        {
            DefaultDimension ??= dimension;
            _dimensions.Add(dimension.UniqueId, dimension);
        }
        public virtual bool RemoveDimension(Dimension dimension) => _dimensions.Remove(dimension.UniqueId);
        protected virtual void Tick(ulong currentTick)
        {
            foreach (var dimension in Dimensions) Dimension.RunTick(dimension, currentTick);
        }
        public static void _RunTick(World world, ulong tick) => world.Tick(tick);
        public static void _SetGameFor(World world, Game game) => world.Game = game;
    }
}
