using Enceladus.Core.Entities;
using Enceladus.Core.World;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionService
    {
        void HandleCollisions(IEnumerable<IEntity> entities, Map map);
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

        public void HandleCollisions(IEnumerable<IEntity> entities, Map map)
        {
            // Filter to only collidable entities
            var collidableEntities = entities.OfType<ICollidableEntity>().ToList();

            // Entity-to-cell collisions
            var entityToCellCollisions = _collisionChecker.CheckEntitiesToCells(collidableEntities, map);
            foreach (var collision in entityToCellCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
            }

            // Entity-to-entity collisions
            var entityToEntityCollisions = _collisionChecker.CheckEntitiesToEntities(collidableEntities);
            foreach (var collision in entityToEntityCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
            }
        }
    }
}
