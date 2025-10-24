using Enceladus.Core.Entities.Weapons.WeaponControllers;

namespace Enceladus.Core.Entities.Weapons
{
    public interface IWeaponControlService
    {
        void 
            ApplyWeaponControl(WeaponMount mount, float deltaTime);
    }

    public class WeaponControlService : IWeaponControlService
    {
        private readonly IMouseWeaponController _mouseController;

        public WeaponControlService(IMouseWeaponController mouseController)
        {
            _mouseController = mouseController;
        }

        public void ApplyWeaponControl(WeaponMount mount, float deltaTime)
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
    }
}
