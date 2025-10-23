using Enceladus.Core.Entities.Weapons.WeaponControllers;
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
        private readonly IMouseWeaponController _mouseController;

        public WeaponService(IEntityRegistry entityRegistry, IMouseWeaponController mouseController)
        {
            _entityRegistry = entityRegistry;
            _mouseController = mouseController;
        }

        public void Update(float deltaTime)
        {
            foreach (var armed in _entityRegistry.ArmedEntities)
            {
                foreach (var mount in armed.WeaponMounts)
                {
                    if (mount.EquippedWeapon == null) continue;

                    UpdateWeaponPosition(mount, armed.Position, armed.Rotation);
                    ApplyWeaponControl(mount, deltaTime);
                }
            }
        }

        private void ApplyWeaponControl(WeaponMount mount, float deltaTime)
        {
            switch (mount.ControllerType)
            {
                case WeaponControllerType.Mouse:
                    _mouseController.Update(mount, deltaTime);
                    break;
                case WeaponControllerType.Fixed:
                    // TODO: implement FixedWeaponController
                    break;
                case WeaponControllerType.TrackTarget:
                    // TODO: implement TrackingWeaponController
                    break;
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
