using Enceladus.Core.World;
using Enceladus.Entities;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionService
    {
        void HandleCollisions(IEnumerable<IEntity> entities, Map map);
    }

    public class CollisionService : ICollisionService
    {
        private readonly ICollisionCheckService _collisionCheckService;
        private readonly ICollisionResolverService _collisionResolverService;

        public CollisionService(ICollisionCheckService collisionCheckService, ICollisionResolverService collisionResolverService)
        {
            _collisionCheckService = collisionCheckService;
            _collisionResolverService = collisionResolverService;
        }

        public void HandleCollisions(IEnumerable<IEntity> entities, Map map)
        {
            // Filter to only collidable entities
            var collidableEntities = entities.OfType<ICollidableEntity>().ToList();

            // Entity-to-cell collisions
            var entityToCellCollisions = _collisionCheckService.CheckEntitiesToCells(collidableEntities, map);
            foreach (var collision in entityToCellCollisions)
            {
                _collisionResolverService.ResolveCollision(collision);
            }

            // Entity-to-entity collisions
            var entityToEntityCollisions = _collisionCheckService.CheckEntitiesToEntities(collidableEntities);
            foreach (var collision in entityToEntityCollisions)
            {
                _collisionResolverService.ResolveCollision(collision);
            }
        }
    }
}
