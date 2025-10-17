using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionResolver
    {
        void ResolveCollision(EntityToCellCollisionResult collision);
        void ResolveCollision(EntityToEntityCollisionResult collision);
    }

    public class CollisionResolver : ICollisionResolver
    {
        private readonly IConfigService _configService;

        public CollisionResolver(IConfigService configService)
        {
            _configService = configService;
        }
        public void ResolveCollision(EntityToCellCollisionResult collision)
        {
            collision.Entity.Position += collision.CollisionNormal * collision.PenetrationDepth;

            if (collision.Entity is IMoveableEntity moveableEntity)
            {
                moveableEntity.Velocity = Vector2.Zero;
            }
        }

        public void ResolveCollision(EntityToEntityCollisionResult collision)
        {
            // 1. Position separation (simple equal split)
            float halfDepth = collision.PenetrationDepth / 2f;
            collision.Entity.Position += collision.CollisionNormal * halfDepth;
            collision.OtherEntity.Position -= collision.CollisionNormal * halfDepth;

            //todo: if only one entity is movable 
            // 2. Velocity bounce (mass-based impulse) - only if both are moveable
            if (collision.Entity is IMoveableEntity moveable1 && collision.OtherEntity is IMoveableEntity moveable2)
            {
                // Calculate relative velocity along collision normal
                Vector2 relativeVelocity = moveable1.Velocity - moveable2.Velocity;
                float velocityAlongNormal = Vector2.Dot(relativeVelocity, collision.CollisionNormal);

                // Don't bounce if entities are moving apart
                if (velocityAlongNormal > 0)
                    return;

                // Calculate bounce impulse
                float restitution = _configService.Config.Physics.RestitutionCoefficient;
                float impulseScalar = -(1 + restitution) * velocityAlongNormal;
                impulseScalar /= (1 / moveable1.Mass + 1 / moveable2.Mass);

                Vector2 impulse = collision.CollisionNormal * impulseScalar;

                // Apply impulse to both entities
                moveable1.Velocity += impulse / moveable1.Mass;
                moveable2.Velocity -= impulse / moveable2.Mass;
            }
        }
    }
}
