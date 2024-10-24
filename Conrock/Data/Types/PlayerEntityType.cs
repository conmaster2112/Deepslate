using ConMaster.Bedrock.Data.Components.Entities;
using ConMaster.Bedrock.Data.Components.Entities.Attributes;

namespace ConMaster.Bedrock.Data.Types
{
    internal class PlayerEntityType(): EntityType("minecraft:player")
    {
        public static readonly PlayerEntityType Instance = new();
        public override void CreateEntityComponents(Entity entity, IDictionary<int, EntityComponent> keys)
        {
            base.CreateEntityComponents(entity, keys);
            HealthComponent health = new(entity)
            {
                Default = 20,
                DefaultMaxValue = 20,
                DefaultMinValue = 0,
                Current = 30,
            };
            MovementComponent movement = new(entity)
            {
                Default = 0.1f,
                DefaultMaxValue = float.MaxValue,
                DefaultMinValue = float.MinValue,
                Current = 0.1f,
            };
            FallDamageComponent falldamage = new(entity)
            {
                Default = 1,
                DefaultMaxValue = 20,
                DefaultMinValue = 20,
                Current = 1,
            };
            UnderwaterMovementComponent waterComponent = new(entity)
            {
                Default = 0.05f,
                DefaultMaxValue = float.MaxValue,
                DefaultMinValue = float.MinValue,
                Current = 0.05f,
            };
            LavaMovementComponent lavaComponent = new(entity)
            {
                Default = 0.05f,
                DefaultMaxValue = float.MaxValue,
                DefaultMinValue = float.MinValue,
                Current = 0.05f,
            };
            keys[health.HashId] = health;
            keys[movement.HashId] = movement;
            keys[falldamage.HashId] = falldamage;
            keys[waterComponent.HashId] = waterComponent;
            keys[lavaComponent.HashId] = lavaComponent;

            PlayerExperienceComponent exp = new(entity)
            {
                Default = 0f,
                DefaultMaxValue = 1,
                DefaultMinValue = 0,
                Current = 0.5f,
            };
            PlayerLevelComponent level = new(entity)
            {
                Default = 0f,
                DefaultMaxValue = 2<<12,
                DefaultMinValue = 0,
                Current = 5f,
            };
            PlayerHungerComponent hunger = new(entity)
            {
                Default = 0f,
                DefaultMaxValue = 20,
                DefaultMinValue = 0,
                Current = 10,
            };
            keys[exp.HashId] = exp;
            keys[level.HashId] = level;
            keys[hunger.HashId] = hunger;
        }
    }
}
