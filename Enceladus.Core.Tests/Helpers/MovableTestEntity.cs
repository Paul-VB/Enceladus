using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.Tests.Helpers
{
    public class MovableTestEntity : MovableEntity
    {
        public MovableTestEntity()
        {
            Guid = Guid.NewGuid();
            Hitbox = new RectHitbox(new(1, 1)); // Default hitbox
        }

        public override IHitbox Hitbox { get; set; }

        public void Accelerate(Vector2 force, float deltaTime) { }

        public void ApplyTorque(float torque, float deltaTime) { }

        public override void Update(float deltaTime) { }
    }
}
