using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Tests.Entities.Weapons
{
    public class TestWeapon : Weapon
    {
        public override float MuzzleVelocity => 10f;
        public override float FireRate { get; set; } = 2f;
        public override ProjectileType ProjectileType { get; set; } = ProjectileType.Bullet;
        public override SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.DefaultEntity;

    }
}
