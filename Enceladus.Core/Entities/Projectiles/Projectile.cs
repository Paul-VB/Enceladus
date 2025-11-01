using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Time;

namespace Enceladus.Core.Entities.Projectiles
{
    public abstract class Projectile : MovableEntity, IIdentifyFriendFoe
    {
        public IArmed Owner { get; set; }
        public List<int> IffCodes { get; set; } = new();
        public abstract float TimeToLive { get; set; }
        public float SpawnTime { get; set; }
        public ScheduledAction DespawnAction { get; set; }
    }
}
