using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Entities.Weapons
{
    public interface IWeaponService
    {
        void Update(float deltaTime);
    }

    public class WeaponService : IWeaponService
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IWeaponControlService _weaponControlService;
        private readonly ITimeService _timeService;

        public WeaponService(IEntityRegistry entityRegistry, IWeaponControlService weaponControlService, ITimeService timeService)
        {
            _entityRegistry = entityRegistry;
            _weaponControlService = weaponControlService;
            _timeService = timeService;
        }

        public void Update(float deltaTime)
        {
            foreach (var armed in _entityRegistry.ArmedEntities)
            {
                foreach (var mount in armed.WeaponMounts)
                {
                    if (mount.EquippedWeapon == null) continue;

                    UpdateWeaponPosition(mount, armed.Position, armed.Rotation);
                    _weaponControlService.ApplyWeaponControl(mount, deltaTime);
                }
            }

            // Cleanup expired projectiles
            CleanupExpiredProjectiles();
        }

        private void CleanupExpiredProjectiles()
        {
            var gameTime = _timeService.GameTime;
            var projectiles = _entityRegistry.Projectiles.ToList(); // Copy to avoid modification during iteration

            foreach (var projectile in projectiles)
            {
                if (gameTime - projectile.SpawnTime >= projectile.TimeToLive)
                {
                    _entityRegistry.Unregister(((Entity)projectile).Guid);
                }
            }
        }

        private void UpdateWeaponPosition(WeaponMount mount, Vector2 parentPosition, float parentRotation)
        {
            if (mount.EquippedWeapon == null) return;

            mount.EquippedWeapon.Position = GeometryHelper.TransformToWorldSpace(
                mount.Offset, parentPosition, parentRotation);
        }
    }
}
