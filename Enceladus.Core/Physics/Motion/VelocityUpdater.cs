using Enceladus.Utils;
using System;
using System.Numerics;

namespace Enceladus.Core.Physics.Motion
{
    /// <summary>
    /// Low-level physics engine that applies forces, drag, and updates entity velocities/positions.
    /// This is the "muscle" - pure physics math with no orchestration logic.
    /// </summary>
    public interface IVelocityUpdater
    {
        void ApplyForce(IMovable entity, Vector2 force, float deltaTime);
        void ApplyTorque(IMovable entity, float torque, float deltaTime);
        void ApplyDrag(IMovable entity, float deltaTime);
        void ApplyThresholds(IMovable entity);
        void UpdatePosition(IMovable entity, float deltaTime);
        void UpdateRotation(IMovable entity, float deltaTime);
    }

    public class VelocityUpdater : IVelocityUpdater
    {
        // Thresholds to prevent floating point drift
        private const float VELOCITY_THRESHOLD = 0.05f;
        private const float ANGULAR_VELOCITY_THRESHOLD = 0.05f;

        public void ApplyForce(IMovable entity, Vector2 force, float deltaTime)
        {
            if (entity.Mass <= 0f)
                throw new InvalidOperationException($"Mass must be positive. Current value: {entity.Mass}");

            Vector2 acceleration = force / entity.Mass;
            entity.Velocity += acceleration * deltaTime;
        }

        public void ApplyTorque(IMovable entity, float torque, float deltaTime)
        {
            if (entity.Mass <= 0f)
                throw new InvalidOperationException($"Mass must be positive. Current value: {entity.Mass}");

            float angularAcceleration = torque / entity.Mass;
            entity.AngularVelocity += angularAcceleration * deltaTime;
        }

        public void ApplyDrag(IMovable entity, float deltaTime)
        {
            var dragForce = -entity.Velocity * entity.Drag;
            ApplyForce(entity, dragForce, deltaTime);

            var angularDragTorque = -entity.AngularVelocity * entity.AngularDrag;
            ApplyTorque(entity, angularDragTorque, deltaTime);
        }

        public void ApplyThresholds(IMovable entity)
        {
            // Zero out very small velocities to prevent floating point drift
            if (entity.Velocity.Length() < VELOCITY_THRESHOLD)
                entity.Velocity = Vector2.Zero;

            // Zero out very small angular velocities to prevent floating point drift
            if (Math.Abs(entity.AngularVelocity) < ANGULAR_VELOCITY_THRESHOLD)
                entity.AngularVelocity = 0f;
        }

        public void UpdatePosition(IMovable entity, float deltaTime)
        {
            entity.Position += entity.Velocity * deltaTime;
        }

        public void UpdateRotation(IMovable entity, float deltaTime)
        {
            entity.Rotation += entity.AngularVelocity * deltaTime;
            entity.Rotation = AngleHelper.ClampAngle0To360(entity.Rotation);
        }
    }
}
