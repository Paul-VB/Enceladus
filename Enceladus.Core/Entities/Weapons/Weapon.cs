using Enceladus.Core.Input;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public abstract class Weapon : Entity, ISpriteRendered
    {
        public required IArmed Owner { get; set; }  // Who owns this weapon (for IFF, kill credit)
        public float FireRate { get; set; } = 1f;  // Shots per second
        private float _timeSinceLastShot = 0f;

        public SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.DefaultEntity; //todo, make a weapon sprite
        public SpriteModifiers SpriteModifiers { get; set; } = new();

        public override void Update(float deltaTime)
        {
            // Position and rotation updated by WeaponMount
            _timeSinceLastShot += deltaTime;
        }

        public void TryFire()
        {
            if (_timeSinceLastShot < 1f / FireRate) return;  // Still on cooldown

            Fire();
            _timeSinceLastShot = 0f;
        }

        protected abstract void Fire();  // Each weapon type implements firing logic
    }
}
