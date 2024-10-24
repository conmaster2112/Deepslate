using ConMaster.Bedrock.Data.Types;
using ConMaster.Bedrock.Engine;
using System.Diagnostics;

namespace ConMaster.Bedrock.Level
{
    public class World
    {
        protected Dictionary<string, Dimension> _dimensions = [];



        public readonly Game Game;
        public ulong CurrentTick => Game.CurrentTick;
        public IReadOnlyCollection<Dimension> Dimensions => _dimensions.Values;



        public World(Game game)
        {
            Game = game;
            AddDimension(new Dimension("custom:overworld", DimensionTypes.Overworld, this));
        }
        public Dimension? GetDimension(string id) { _dimensions.TryGetValue(id, out Dimension? dimension); return dimension; }

        public void AddDimension(Dimension dimension) => _dimensions.Add(dimension.UniqueId, dimension);
        internal void Tick(ulong currentTick)
        {
            foreach (var dimension in Dimensions) dimension.Tick(currentTick);
        }
    }
}
