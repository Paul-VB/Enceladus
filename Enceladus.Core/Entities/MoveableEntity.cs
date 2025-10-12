using System.Numerics;

namespace Enceladus.Entities
{
    public interface IMoveable
    {
        Vector2 Velocity { get; set; }
        float Friction { get; set; }
        void Accelerate(Vector2 force);
    }

    public abstract class MoveableEntity : Entity, IMoveable
    {
        private const float _minVelocityThreshold = 0.05f;
        public Vector2 Velocity { get; set; }
        public float Friction { get; set; } = 0.9f;

        protected MoveableEntity()
        {
            Velocity = Vector2.Zero;
        }

        public virtual void Accelerate(Vector2 force)
        {
            Velocity += force;
        }

        public override void Update(float deltaTime)
        {
            UpdateMovement(deltaTime);
        }

        protected virtual void UpdateMovement(float deltaTime)
        {
            Position += Velocity * deltaTime;
            Velocity *= Friction;

            var X = Math.Abs(Velocity.X) < _minVelocityThreshold ? 0 : Velocity.X;
            var Y = Math.Abs(Velocity.Y) < _minVelocityThreshold ? 0 : Velocity.Y;
            Velocity = new Vector2(X, Y);
        }
    }
}