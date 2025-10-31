using Enceladus.Core.Input;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public abstract class Weapon : Entity, ISpriteRendered
    {
        public IArmed Owner { get; set; } = null!;
        public abstract float FireRate { get; set; }  //todo: make this a Rounds Per Minute figure? or a rounds per second number?
        public abstract ProjectileType ProjectileType { get; set; }
        public abstract float MuzzleVelocity { get; }
        public float LastShotTime { get; set; } = 0f; 
        public abstract SpriteDefinition CurrentSprite { get; set; }
        public SpriteModifiers SpriteModifiers { get; set; } = new();
    }
}
