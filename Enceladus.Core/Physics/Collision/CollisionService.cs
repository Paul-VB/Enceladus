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
        private readonly IImpactHandlerService _impactHandlerService;

        public CollisionService(ICollisionChecker collisionChecker, ICollisionResolver collisionResolver, IImpactHandlerService impactHandlerService)
        {
            _collisionChecker = collisionChecker;
            _collisionResolver = collisionResolver;
            _impactHandlerService = impactHandlerService;
        }

        public void HandleCollisions(Map map)
        {
            // Entity-to-cell collisions
            var entityToCellCollisions = _collisionChecker.CheckEntitiesToCells();
            foreach (var collision in entityToCellCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
                _impactHandlerService.HandleImpact(collision); 
            }

            // Entity-to-entity collisions
            var entityToEntityCollisions = _collisionChecker.CheckEntitiesToEntities();
            foreach (var collision in entityToEntityCollisions)
            {
                _collisionResolver.ResolveCollision(collision);
                _impactHandlerService.HandleImpact(collision); 
            }
        }
    }
}
