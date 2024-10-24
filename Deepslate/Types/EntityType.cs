namespace ConMaster.Deepslate.Types
{
    public class EntityType: Type<EntityType>
    {
        public EntityType(string id): base(id)
        {

        }
    }
    public sealed class EntityTypes: Types<EntityType>
    {
        public static readonly EntityType Player = PlayerEntityType.Instance;
        public static readonly EntityType ItemStack = new("minecraft:item");
    }
}
