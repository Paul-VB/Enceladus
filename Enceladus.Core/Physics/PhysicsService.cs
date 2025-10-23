using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.World;

namespace Enceladus.Core.Physics
{
    public interface IPhysicsService
    {
        void Update(float deltaTime);
    }

    public class PhysicsService : IPhysicsService
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IWorldService _worldService;
        private readonly ICollisionService _collisionService;

        public PhysicsService(IEntityRegistry entityRegistry, IWorldService worldService,
            ICollisionService collisionService)
        {
            _entityRegistry = entityRegistry;
            _worldService = worldService;
            _collisionService = collisionService;
        }

        public void Update(float deltaTime)
        {
            // Update all entities (movement, rotation, etc.)
            foreach (var entity in _entityRegistry.Entities.Values)
            {
                entity.Update(deltaTime);
            }

            // Handle collisions (detection + resolution)
            _collisionService.HandleCollisions(_worldService.CurrentMap);
        }
    }
}
