using Enceladus.Core.Physics.Motion.MotionControllers;
using Enceladus.Core.Time;

namespace Enceladus.Core.Entities.Weapons
{
    public abstract class Projectile : MovableEntity, IIdentifyFriendFoe
    {
        public IArmed Owner { get; set; }
        public List<int> IffCodes { get; set; } = new();
        public abstract float TimeToLive { get; set; }
        public float SpawnTime { get; set; }
        public override MotionControllerType MotionControllerType { get; set; } = MotionControllerType.None;
        public ScheduledAction DespawnAction { get; set; }
    }
}
