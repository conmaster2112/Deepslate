namespace ConMaster.Bedrock.Data.Components.Entities
{
    public abstract class EntityComponent(string id, Entity entity): Component(id)
    {
        public Entity Entity { get; protected init; } = entity;
    }
}
