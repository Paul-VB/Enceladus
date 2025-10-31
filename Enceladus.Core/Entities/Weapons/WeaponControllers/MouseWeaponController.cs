using Enceladus.Core.Input;
using Enceladus.Core.Utils;
using Enceladus.Utils;

namespace Enceladus.Core.Entities.Weapons.WeaponControllers
{
    public interface IMouseWeaponController
    {
        void Update(WeaponMount mount, float deltaTime);
    }

    public class MouseWeaponController : BaseWeaponController, IMouseWeaponController
    {
        private readonly IInputReader _inputReader;

        public MouseWeaponController(IInputReader inputReader, ITimeService timeService, IEntityFactory entityFactory)
            : base(timeService, entityFactory)
        {
            _inputReader = inputReader;
        }

        public override void Update(WeaponMount mount, float deltaTime)
        {
            var weapon = mount.EquippedWeapon;
            if (weapon == null) return;

            AimWeaponAtMouse(mount);

            if (_inputReader.GetFireWeaponInput() && CanFire(weapon))
            {
                Fire(weapon);
            }
        }

        private void AimWeaponAtMouse(WeaponMount mount)
        {
            var mouseWorldPos = _inputReader.GetMouseWorldPosition();
            var weaponPos = mount.EquippedWeapon!.Position;

            var direction = mouseWorldPos - weaponPos;
            var angleRadians = MathF.Atan2(direction.Y, direction.X);
            var angleDegrees = AngleHelper.RadToDeg(angleRadians);

            mount.EquippedWeapon.Rotation = angleDegrees;
        }
    }
}
