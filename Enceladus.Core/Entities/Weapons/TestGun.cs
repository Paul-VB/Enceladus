using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public class TestGun : Weapon
    {
        public TestGun()
        {
            FireRate = 2f; // 2 shots per second
            CurrentSprite = SpriteDefinitions.Entities.TestGun;
        }

        protected override void Fire()
        {
            //todo: For now, just log that we fired
            // Later: spawn projectile, play sound, etc.
            Console.WriteLine($"TestGun fired at position {Position}, rotation {Rotation}");
        }
    }
}
