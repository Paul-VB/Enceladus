using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.Entities
{

    public abstract class Entity : ICollidable
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public abstract IHitbox Hitbox { get; set; }
        public virtual bool CollisionEnabled { get; set; } = true;
    }
}
