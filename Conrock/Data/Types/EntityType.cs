using ConMaster.Bedrock.Data.Components.Entities;

namespace ConMaster.Bedrock.Data.Types
{
    public class EntityType: Type<EntityType>
    {
        public EntityType(string id): base(id)
        {

        }
        public virtual void CreateEntityComponents(Entity entity, IDictionary<int, EntityComponent> keys)
        {

        }
    }
    public sealed class EntityTypes: Types<EntityType>
    {
        public static readonly EntityType Player = PlayerEntityType.Instance;
        public static readonly EntityType ItemStack = new("minecraft:item");
    }
}
