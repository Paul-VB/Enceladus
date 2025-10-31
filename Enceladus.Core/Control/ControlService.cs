using Enceladus.Core.Entities.Weapons;

namespace Enceladus.Core.Control
{
    public interface IControlService
    {
        void Update(float deltaTime);
    }

    public class ControlService : IControlService
    {
        private readonly IMotionControlService _motionControlService;
        private readonly IWeaponService _weaponService;

        public ControlService(IMotionControlService motionControlService, IWeaponService weaponService)
        {
            _motionControlService = motionControlService;
            _weaponService = weaponService;
        }

        public void Update(float deltaTime)
        {
            // Motion control (player movement, AI, etc.)
            _motionControlService.UpdateAll(deltaTime);

            // Weapon control (aiming, firing)
            _weaponService.Update(deltaTime);
        }
    }
}
