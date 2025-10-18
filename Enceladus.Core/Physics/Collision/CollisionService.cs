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
        private readonly IEntityRegistry _entityRegistry;

        public CollisionService(ICollisionChecker collisionChecker, ICollisionResolver collisionResolver, IEntityRegistry entityRegistry)
        {
            _collisionChecker = collisionChecker;
            _collisionResolver = collisionResolver;
            _entityRegistry = entityRegistry;
        }

        public void HandleCollisions(Map map)
        {
            // Entity-to-cell collisions
            var entityToCellCollisions = _collisionChecker.CheckEntitiesToCells(_entityRegistry.MovableEntities, map);
            foreach (var collision in entityToCellCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
            }

            //todo: maybe we should just inject entityRegistry here? its a pretty simple class anyway, its just a dict and some lists.
            // Entity-to-entity collisions
            var entityToEntityCollisions = _collisionChecker.CheckEntitiesToEntities(_entityRegistry);
            foreach (var collision in entityToEntityCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
            }
        }
    }
}
