using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionResolver
    {
        void ResolveCollision(BaseCollisionResult collision);
    }

    public class CollisionResolver : ICollisionResolver
    {
        private readonly IConfigService _configService;

        public CollisionResolver(IConfigService configService)
        {
            _configService = configService;
        }

        public void ResolveCollision(BaseCollisionResult collision)
        {
            if(collision.OtherObject is MovableEntity otherObject)
            {
                ResolveEntityToMovable(collision, otherObject);

            } else
            {
                ResolveEntityToStatic(collision);
            }
        }

        //entity to cell
        private void ResolveEntityToStatic(BaseCollisionResult collision)
        {
            collision.Entity.Position += collision.CollisionNormal * collision.PenetrationDepth;
            collision.Entity.Velocity = Vector2.Zero;
        }


        //entity to entity
        private void ResolveEntityToMovable(BaseCollisionResult collision, MovableEntity otherEntity)
        {
            var entity = collision.Entity;
            // 1. Position separation (simple equal split)
            var halfDepth = collision.PenetrationDepth / 2f;
            entity.Position += collision.CollisionNormal * halfDepth;
            otherEntity.Position -= collision.CollisionNormal * halfDepth;
            // 2. Velocity bounce (mass-based impulse)

                // Calculate relative velocity along collision normal
            var relativeVelocity = entity.Velocity - otherEntity.Velocity;
            var velocityAlongNormal = Vector2.Dot(relativeVelocity, collision.CollisionNormal);

            // Don't bounce if entities are moving apart
            if (velocityAlongNormal > 0)
                return;

            // Calculate bounce impulse
            var restitution = _configService.Config.Physics.RestitutionCoefficient;
            var impulseScalar = -(1 + restitution) * velocityAlongNormal;
            impulseScalar /= (1 / entity.Mass + 1 / otherEntity.Mass);

            Vector2 impulse = collision.CollisionNormal * impulseScalar;

            // Apply impulse to both entities
            entity.Velocity += impulse / entity.Mass;
            otherEntity.Velocity -= impulse / otherEntity.Mass;
            
        }
    }
}
