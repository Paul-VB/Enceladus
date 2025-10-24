using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public class TestGun : Weapon
    {
        public override float MuzzleVelocity => 50f;
        public override float FireRate { get; set; } = 2f;
        public override ProjectileType ProjectileType { get; set; } = ProjectileType.Bullet;
        public override SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.TestGun;
    }
}
