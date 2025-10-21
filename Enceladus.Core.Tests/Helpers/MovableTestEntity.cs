using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Physics.Motion;
using System.Numerics;

namespace Enceladus.Core.Tests.Helpers
{
    public class MovableTestEntity : Entity, IMovable
    {
        public MovableTestEntity()
        {
            Guid = Guid.NewGuid();
            Hitbox = new RectHitbox(new(1, 1)); // Default hitbox
        }

        public override IHitbox Hitbox { get; set; }
        public float Mass { get; set; }
        public Vector2 Velocity { get; set; }
        public float Drag { get; set; }
        public float AngularVelocity { get; set; }
        public float AngularDrag { get; set; }

        public void Accelerate(Vector2 force, float deltaTime) { }

        public void ApplyTorque(float torque, float deltaTime) { }

        public override void Update(float deltaTime) { }
    }
}
