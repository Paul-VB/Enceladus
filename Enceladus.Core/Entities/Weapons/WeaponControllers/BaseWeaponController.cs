using Enceladus.Core.Time;

namespace Enceladus.Core.Entities.Weapons.WeaponControllers
{
    /// <summary>
    /// Base class for weapon controllers that provides common firing logic.
    /// </summary>
    public abstract class BaseWeaponController
    {
        protected readonly ITimeService _timeService;
        protected readonly IEntityFactory _entityFactory;

        protected BaseWeaponController(ITimeService timeService, IEntityFactory entityFactory)
        {
            _timeService = timeService;
            _entityFactory = entityFactory;
        }

        protected bool CanFire(Weapon weapon)
        {
            var timeSinceLastShot = _timeService.GameTime - weapon.LastShotTime;
            return timeSinceLastShot >= 1f / weapon.FireRate;
        }

        protected void Fire(Weapon weapon)
        {
            _entityFactory.CreateProjectile(weapon);
            weapon.LastShotTime = _timeService.GameTime;
        }

        public abstract void Update(WeaponMount mount, float deltaTime);
    }
}
