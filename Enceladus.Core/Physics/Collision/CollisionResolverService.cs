using Enceladus.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionResolverService
    {
        void ResolveCollision(EntityToCellCollisionResult collision);
        void ResolveCollision(EntityToEntityCollisionResult collision);
    }

    public class CollisionResolverService : ICollisionResolverService
    {
        public void ResolveCollision(EntityToCellCollisionResult collision)
        {
            collision.Entity.Position += collision.CollisionNormal * collision.PenetrationDepth;

            if (collision.Entity is IMoveable moveableEntity)
            {
                moveableEntity.Velocity = Vector2.Zero;
                //moveableEntity.AngularVelocity = 0f;
            }
        }

        public void ResolveCollision(EntityToEntityCollisionResult collision)
        {
            throw new NotImplementedException();
        }
    }
}
