namespace ConMaster.Bedrock.Data.Components.Entities.Attributes
{
    public class HealthComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.Health, entity);
    public class LuckComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.Luck, entity);
    public class PlayerSaturationComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.PlayerSaturation, entity);
    public class PlayerExhaustionComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.PlayerExhaustion, entity);
    public class PlayerExperienceComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.PlayerExperience, entity);
    public class PlayerLevelComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.PlayerLevel, entity);
    public class PlayerHungerComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.PlayerHunger, entity);
    public class AbsorptionComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.Absorption, entity);
    public class AttackDamageComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.AttackDamage, entity);
    public class FallDamageComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.FallDamage, entity);
    public class LavaMovementComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.LavaMovement, entity);
    public class UnderwaterMovementComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.UnderwaterMovement, entity);
    public class MovementComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.Movement, entity);
    public class KnockbackResistenceComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.KnockbackResistence, entity);
    public class ZombieSpawnReinforcementsComponent(Entity entity): AttributeEntityComponent(VanillaEntityAttributes.ZombieSpawnReinforcements, entity);
}
