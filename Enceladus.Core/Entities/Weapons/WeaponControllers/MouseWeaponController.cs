using Enceladus.Core.Input;
using Enceladus.Utils;

namespace Enceladus.Core.Entities.Weapons.WeaponControllers
{
    public interface IMouseWeaponController
    {
        void Update(WeaponMount mount, float deltaTime);
    }
    public class MouseWeaponController : IMouseWeaponController
    {
        private readonly IInputReader _inputReader;
        private readonly IEntityFactory _entityFactory;

        public MouseWeaponController(IInputReader inputReader, IEntityFactory entityFactory)
        {
            _inputReader = inputReader;
            _entityFactory = entityFactory;
        }

        public void Update(WeaponMount mount, float deltaTime)
        {
            if (mount.EquippedWeapon == null) return;

            AimWeaponAtMouse(mount);

            if (_inputReader.GetFireWeaponInput() && mount.EquippedWeapon.CanFire())
            {
                _entityFactory.CreateProjectile(mount.EquippedWeapon);
                mount.EquippedWeapon.ResetCooldown();
            }
        }

        private void AimWeaponAtMouse(WeaponMount mount)
        {
            var mouseWorldPos = _inputReader.GetMouseWorldPosition();
            var weaponPos = mount.EquippedWeapon!.Position;

            // Calculate angle from weapon to mouse
            var direction = mouseWorldPos - weaponPos;
            var angleRadians = MathF.Atan2(direction.Y, direction.X);
            var angleDegrees = AngleHelper.RadToDeg(angleRadians);

            // Apply rotation to weapon
            mount.EquippedWeapon.Rotation = angleDegrees;
        }
    }
}
