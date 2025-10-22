using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionResolver
    {
        void ResolveCollision(CollisionResult collision);
    }

    public class CollisionResolver : ICollisionResolver
    {
        private readonly IConfigService _configService;

        public CollisionResolver(IConfigService configService)
        {
            _configService = configService;
        }

        public void ResolveCollision(CollisionResult collision)
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
        private void ResolveEntityToStatic(CollisionResult collision)
        {
            //move the movable one away from the static
            collision.Entity.Position += collision.CollisionNormal * collision.PenetrationDepth;


            var velocityAlongNormal = Vector2.Dot(collision.Entity.Velocity, collision.CollisionNormal);
            if (ShouldSkipCollision(velocityAlongNormal))
                return;

            var restitution = _configService.Config.Physics.RestitutionCoefficient;
            var impulse = CalculateImpulse(restitution, velocityAlongNormal, collision.CollisionNormal);

            collision.Entity.Velocity += impulse;
        }


        //entity to entity
        private void ResolveEntityToMovable(CollisionResult collision, MovableEntity otherEntity)
        {
            var entity = collision.Entity;

            // Position separation (simple equal split)
            var halfDepth = collision.PenetrationDepth / 2f;
            entity.Position += collision.CollisionNormal * halfDepth;
            otherEntity.Position -= collision.CollisionNormal * halfDepth;

            // Velocity bounce (mass-based impulse)
            var relativeVelocity = entity.Velocity - otherEntity.Velocity;
            var velocityAlongNormal = Vector2.Dot(relativeVelocity, collision.CollisionNormal);

            if (ShouldSkipCollision(velocityAlongNormal))
                return;

            var restitution = _configService.Config.Physics.RestitutionCoefficient;
            var impulse = CalculateImpulse(restitution, velocityAlongNormal, collision.CollisionNormal);
            

            // Apply impulse to both entities
            var totalMass = entity.Mass + otherEntity.Mass;
            var entityMassProportion = entity.Mass / totalMass;
            var otherEntityMassProportion = otherEntity.Mass / totalMass;

            entity.Velocity += impulse * otherEntityMassProportion;
            otherEntity.Velocity -= impulse * entityMassProportion;
        }

        private bool ShouldSkipCollision(float velocityAlongNormal)
        {
            return velocityAlongNormal > 0;
        }

        private Vector2 CalculateImpulse(float restitution, float velocityAlongNormal, Vector2 collisionNormal)
        {
            var impulseScalar = -(1 + restitution) * velocityAlongNormal;
            var impulse = collisionNormal * impulseScalar;
            return impulse;
        }
    }
}
