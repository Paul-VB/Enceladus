using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.Entities
{

    public abstract class Entity : ICollidable
    {
        protected Entity()
        {

        }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public abstract IHitbox Hitbox { get; set; }
        public abstract void Update(float deltaTime);
    }
}
