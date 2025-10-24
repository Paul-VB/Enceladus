using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public class TestGun : Weapon
    {
        public override float MuzzleVelocity => 50f;

        public TestGun()
        {
            FireRate = 2f; // 2 shots per second
            ProjectileType = ProjectileType.Bullet;
            CurrentSprite = SpriteDefinitions.Entities.TestGun;
        }
    }
}
