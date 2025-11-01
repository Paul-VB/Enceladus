using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Time;
using Enceladus.Core.MotionControl.PlayerMotion;

namespace Enceladus.Core.Entities.Projectiles
{
    public abstract class Projectile : MovableEntity, IIdentifyFriendFoe
    {
        public IArmed Owner { get; set; }
        public List<int> IffCodes { get; set; } = new();
        public abstract float TimeToLive { get; set; }
        public float SpawnTime { get; set; }
        public override PlayerMotionControllerType MotionControllerType { get; set; } = PlayerMotionControllerType.None;
        public ScheduledAction DespawnAction { get; set; }
    }
}
