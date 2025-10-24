using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Physics.Motion;
using Enceladus.Utils;

namespace Enceladus.Core.Entities.Weapons
{
    public interface IProjectileFactory
    {
        IProjectile CreateProjectile(Weapon weapon);
    }

    public class ProjectileFactory : IProjectileFactory
    {
        private readonly ITimeService _timeService;

        public ProjectileFactory(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public IProjectile CreateProjectile(Weapon weapon)
        {
            switch (weapon.ProjectileType)
            {
                case ProjectileType.Bullet:
                    return CreateBullet(weapon);
                case ProjectileType.Laser:
                    // TODO: implement laser projectile
                    throw new NotImplementedException("Laser projectile not yet implemented");
                case ProjectileType.Plasma:
                    // TODO: implement plasma projectile
                    throw new NotImplementedException("Plasma projectile not yet implemented");
                case ProjectileType.MiningBeam:
                    // TODO: implement mining beam
                    throw new NotImplementedException("MiningBeam projectile not yet implemented");
                default:
                    throw new ArgumentException($"Unknown projectile type: {weapon.ProjectileType}");
            }
        }

        private void InitializeProjectile<T>(T projectile, Weapon weapon) where T : MovableEntity, IProjectile
        {
            var direction = AngleHelper.DegToNormalVector(weapon.Rotation);
            var velocity = direction * weapon.MuzzleVelocity;

            // Add owner's velocity if owner is movable (inherit inertia)
            if (weapon.Owner is IMovable movableOwner)
            {
                velocity += movableOwner.Velocity;
            }

            projectile.Position = weapon.Position;//todo: make a weapon.bulletSpawnOffset?
            projectile.Rotation = weapon.Rotation;
            projectile.Velocity = velocity;
            projectile.Owner = weapon.Owner;
            projectile.IffCodes = new List<int>(weapon.Owner.IffCodes); // Snapshot owner's IFF codes
            projectile.SpawnTime = _timeService.GameTime;
        }

        private Bullet CreateBullet(Weapon weapon)
        {
            var bullet = new Bullet();
            InitializeProjectile(bullet, weapon);
            return bullet;
        }
    }
}
