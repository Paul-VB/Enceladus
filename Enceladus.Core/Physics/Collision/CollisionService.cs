using Enceladus.Core;
using Enceladus.Core.World;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionService
    {
        void HandleCollisions(Map map);
    }

    public class CollisionService : ICollisionService
    {
        private readonly ICollisionChecker _collisionChecker;
        private readonly ICollisionResolver _collisionResolver;

        public CollisionService(ICollisionChecker collisionChecker, ICollisionResolver collisionResolver)
        {
            _collisionChecker = collisionChecker;
            _collisionResolver = collisionResolver;
        }

        public void HandleCollisions(Map map)
        {
            // Entity-to-cell collisions
            var entityToCellCollisions = _collisionChecker.CheckEntitiesToCells();
            foreach (var collision in entityToCellCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
            }

            // Entity-to-entity collisions
            var entityToEntityCollisions = _collisionChecker.CheckEntitiesToEntities();
            foreach (var collision in entityToEntityCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
            }
        }
    }
}
