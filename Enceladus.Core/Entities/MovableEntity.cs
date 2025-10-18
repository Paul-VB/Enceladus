using Enceladus.Core.Config;
using Enceladus.Core.Physics.Motion;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public abstract class MovableEntity : Entity, IMovable
    {
        private readonly MovableComponent _movableComponent;
        protected readonly IConfigService ConfigService;

        public MovableEntity(IConfigService configService)
        {
            ConfigService = configService;
            _movableComponent = new(ConfigService);
        }

        public Vector2 Velocity
        {
            get => _movableComponent.Velocity;
            set => _movableComponent.Velocity = value;
        }
        public float Mass
        {
            get => _movableComponent.Mass;
            set => _movableComponent.Mass = value;
        }
        public float Drag
        {
            get => _movableComponent.Drag;
            set => _movableComponent.Drag = value;
        }
        public float AngularVelocity
        {
            get => _movableComponent.AngularVelocity;
            set => _movableComponent.AngularVelocity = value;
        }
        public float AngularDrag
        {
            get => _movableComponent.AngularDrag;
            set => _movableComponent.AngularDrag = value;
        }
        public void Accelerate(Vector2 force, float deltaTime) => _movableComponent.Accelerate(force, deltaTime);
        public void ApplyTorque(float torque, float deltaTime) => _movableComponent.ApplyTorque(torque, deltaTime);

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

            if (Velocity.Length() < ConfigService.Config.Physics.MinVelocityThreshold)
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
            if (Math.Abs(AngularVelocity) < ConfigService.Config.Physics.MinAngularVelocityThreshold)
                AngularVelocity = 0f;
        }
    }
}