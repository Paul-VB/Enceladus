using Enceladus.Core.Config;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Motion
{
    //todo: review if we want this component based doodad. currently its only used in movable entity... is it a needless extraction?
    public sealed class MovableComponent : IMovable
    {
        private readonly IConfigService _configService;
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Mass { get; set; }
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public float Drag { get; set; }
        public float AngularVelocity { get; set; }
        public float AngularDrag { get; set; }
        public MovableComponent(IConfigService configService)
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

        public void Update(float deltaTime)
        {
            UpdateMovement(deltaTime);
            UpdateRotation(deltaTime);
        }

        private void UpdateMovement(float deltaTime)
        {
            Position += Velocity * deltaTime;

            var dragForce = -Velocity * Drag;
            Velocity += dragForce * deltaTime;

            if (Velocity.Length() < _configService.Config.Physics.MinVelocityThreshold)
                Velocity = Vector2.Zero;

        }

        private void UpdateRotation(float deltaTime)
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
