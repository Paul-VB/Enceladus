using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Physics.Motion;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public abstract class MovableEntity : Entity, IMovable, ICollidable
    {
        public Vector2 Velocity { get; set; }
        public float Mass { get; set; }
        public float Drag { get; set; }
        public float AngularVelocity { get; set; }
        public float AngularDrag { get; set; }
        public float MinVelocityThreshold { get; set; }
        public float MinAngularVelocityThreshold { get; set; }
        public abstract IHitbox Hitbox { get; set; }

        public void Accelerate(Vector2 force, float deltaTime)
        {
            if (Mass <= 0f)
                throw new InvalidOperationException($"Mass must be positive. Current value: {Mass}");

            Vector2 acceleration = force / Mass;
            Velocity += acceleration * deltaTime;
        }

        public void ApplyTorque(float torque, float deltaTime)
        {
            if (Mass <= 0f)
                throw new InvalidOperationException($"Mass must be positive. Current value: {Mass}");

            float angularAcceleration = torque / Mass;
            AngularVelocity += angularAcceleration * deltaTime;
        }

        public override void Update(float deltaTime)
        {
            UpdateMovement(deltaTime);
            UpdateRotation(deltaTime);
        }

        protected virtual void UpdateMovement(float deltaTime)
        {
            Position += Velocity * deltaTime;

            var dragForce = -Velocity * Drag;
            Velocity += dragForce * deltaTime;

            if (Velocity.Length() < MinVelocityThreshold)
                Velocity = Vector2.Zero;

        }

        protected virtual void UpdateRotation(float deltaTime)
        {
            Rotation += AngularVelocity * deltaTime;
            Rotation = AngleHelper.ClampAngle0To360(Rotation);

            // Apply angular drag
            var angularDragTorque = -AngularVelocity * AngularDrag;
            AngularVelocity += angularDragTorque * deltaTime;

            // Zero out very small angular velocities
            if (Math.Abs(AngularVelocity) < MinAngularVelocityThreshold)
                AngularVelocity = 0f;
        }
    }
}