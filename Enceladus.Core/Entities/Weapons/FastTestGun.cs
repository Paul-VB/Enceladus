using Enceladus.Core.Rendering;
using Raylib_cs;

namespace Enceladus.Core.Entities.Weapons
{
    public class FastTestGun : Weapon
    {
        public override float MuzzleVelocity => 50f;
        public override float FireRate { get; set; } = 10f;
        public override ProjectileType ProjectileType { get; set; } = ProjectileType.Bullet;
        public override SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.TestGun;
        public FastTestGun()
        {
            SpriteModifiers.Tint = Color.Red; // Red tint
        }
    }
}
