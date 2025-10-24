using Enceladus.Core.Input;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public abstract class Weapon : Entity, ISpriteRendered
    {
        public IArmed Owner { get; set; } = null!;   
        public float FireRate { get; set; } = 1f;  //todo: make this a Rounds Per Minute figure? or a rounds per second number?
        public ProjectileType ProjectileType { get; set; } = ProjectileType.None;
        public abstract float MuzzleVelocity { get; } 
        private float _timeSinceLastShot = 0f;

        public SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.DefaultEntity; //todo, make a weapon sprite
        public SpriteModifiers SpriteModifiers { get; set; } = new();

        public override void Update(float deltaTime)
        {
            _timeSinceLastShot += deltaTime;
        }

        public bool CanFire()
        {
            return _timeSinceLastShot >= 1f / FireRate;
        }

        public void ResetCooldown()
        {
            _timeSinceLastShot = 0f;
        }
    }
}
