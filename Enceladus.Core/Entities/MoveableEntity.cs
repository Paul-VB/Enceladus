using Enceladus.Core.Config;
using Enceladus.Utils;
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
        protected readonly IConfigService _configService;
        public virtual Vector2 Velocity { get; set; } = Vector2.Zero;
        public virtual float Mass { get; set; }
        public virtual float Drag { get; set; }
        public virtual float AngularVelocity { get; set; }
        public virtual float AngularDrag{ get; set; }
        protected MoveableEntity(IConfigService configService)
        {
            _configService = configService;

            Init();
        }

        private void Init()
        {
            Mass = _configService.Config.Physics.DefaultMass;
            Drag = _configService.Config.Physics.DefaultDrag;
            AngularDrag = _configService.Config.Physics.DefaultAngularDrag;
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

            if (Velocity.Length() < _configService.Config.Physics.MinVelocityThreshold)
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
            if (Math.Abs(AngularVelocity) < _configService.Config.Physics.MinAngularVelocityThreshold)
                AngularVelocity = 0f;
        }
    }
}