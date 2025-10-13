using System.Numerics;

namespace Enceladus.Entities
{
    public interface IMoveable
    {
        Vector2 Velocity { get; set; }
        float Mass { get; set; }
        float Drag { get; set; }
        float AngularVelocity { get; set; }
        float AngularDrag { get; set; }
        void Accelerate(Vector2 force, float deltaTime);
        void ApplyTorque(float torque, float deltaTime);
    }

    public abstract class MoveableEntity : Entity, IMoveable
    {
        private const float _minVelocityThreshold = 0.05f;
        private const float _minAngularVelocityThreshold = 0.05f;
        public virtual Vector2 Velocity { get; set; }
        public virtual float Mass { get; set; } = 1f;
        public virtual float Drag { get; set; } = 0.95f;
        public virtual float AngularVelocity { get; set; }
        public virtual float AngularDrag{ get; set; } = 0.95f;

        protected MoveableEntity()
        {
            Velocity = Vector2.Zero;
        }

        public virtual void Accelerate(Vector2 force, float deltaTime)
        {
            if(Mass <= 0f)
                throw new InvalidOperationException($"Mass must be positive. Current value: {Mass}");

            Vector2 acceleration = force / Mass;
            Velocity += acceleration * deltaTime;
        }

        public virtual void ApplyTorque(float torque, float deltaTime)
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

            if (Velocity.Length() < _minVelocityThreshold)
                Velocity = Vector2.Zero;

            Console.WriteLine($"Speed: {Velocity.Length():F2}");
        }

        protected virtual void UpdateRotation(float deltaTime)
        {
            Rotation += AngularVelocity * deltaTime;

            // Apply angular drag
            var angularDragTorque = -AngularVelocity * AngularDrag;
            AngularVelocity += angularDragTorque * deltaTime;

            // Zero out very small angular velocities
            if (Math.Abs(AngularVelocity) < _minAngularVelocityThreshold)
                AngularVelocity = 0f;
        }
    }
}