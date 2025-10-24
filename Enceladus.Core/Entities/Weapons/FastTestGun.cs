using Enceladus.Core.Rendering;
using Raylib_cs;

namespace Enceladus.Core.Entities.Weapons
{
    public class FastTestGun : Weapon
    {
        public override float MuzzleVelocity => 50f;

        public FastTestGun()
        {
            FireRate = 10f; // 10 shots per second (5x faster than TestGun)
            ProjectileType = ProjectileType.Bullet;
            CurrentSprite = SpriteDefinitions.Entities.TestGun;
            SpriteModifiers.Tint = Color.Red; // Red tint
        }
    }
}
