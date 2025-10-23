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

        public WeaponService(IEntityRegistry entityRegistry)
        {
            _entityRegistry = entityRegistry;
        }

        public void Update(float deltaTime)
        {
            foreach (var armed in _entityRegistry.ArmedEntities)
            {
                foreach (var mount in armed.WeaponMounts)
                {
                    // Update weapon position based on parent
                    UpdateWeaponPosition(mount, armed.Position, armed.Rotation);

                    // Todo: Update weapon aiming
                    // UpdateWeaponAiming(mount, ...);

                    // Try to fire weapon (weapon handles its own cooldown/logic)
                    mount.EquippedWeapon?.TryFire();
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
