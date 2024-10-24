namespace ConMaster.Bedrock.Data.Types
{
    public class DimensionType : Type<DimensionType>
    {
        internal DimensionType(string id, int networkId): base(id)
        { 
            NetworkId = networkId;
        }
        public int NetworkId { get; init; }
        public int TopBuildLimit { get; init; }
        public int BottomBuildLimit { get; init; }
    }
    public sealed class DimensionTypes : Types<DimensionType>
    {
        public static readonly DimensionType Overworld = new("minecraft:overworld", 0) 
        {
            BottomBuildLimit = -64,
            TopBuildLimit = 320
        };
        public static readonly DimensionType Nether = new("minecraft:nether", 1)
        {
            BottomBuildLimit = 0,
            TopBuildLimit = 128
        };
        public static readonly DimensionType TheEnd = new("minecraft:the_end", 2)
        {
            BottomBuildLimit = 0,
            TopBuildLimit = 128
        };
    }
}
