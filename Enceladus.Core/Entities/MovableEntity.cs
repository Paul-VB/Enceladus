using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Physics.Motion;
using Enceladus.Core.Physics.Motion.MotionControllers;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public abstract class MovableEntity : Entity, IMovable
    {
        public Vector2 Velocity { get; set; }
        public virtual float Mass { get; set; } = 1f;
        public float Drag { get; set; } = 1f;
        public float AngularVelocity { get; set; }
        public float AngularDrag { get; set; } = 1f;
        public abstract MotionControllerType MotionControllerType { get; set; }
    }
}