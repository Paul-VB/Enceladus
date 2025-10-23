using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;

namespace Enceladus.Core.Tests.Helpers
{
    public class StaticTestEntity : Entity, ICollidable
    {
        public StaticTestEntity()
        {
            Guid = Guid.NewGuid();
            Hitbox = new RectHitbox(new(1, 1)); // Default hitbox
        }

        public IHitbox Hitbox { get; set; }
        public override void Update(float deltaTime) { }
    }
}
