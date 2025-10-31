using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Motion;
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
        private readonly IVelocityUpdater _velocityUpdater;

        public PhysicsService(IEntityRegistry entityRegistry, IWorldService worldService, ICollisionService collisionService, IVelocityUpdater velocityUpdater)
        {
            _entityRegistry = entityRegistry;
            _worldService = worldService;
            _collisionService = collisionService;
            _velocityUpdater = velocityUpdater;
        }

        public void Update(float deltaTime)
        {
            // Apply physics to all movable entities
            foreach (var entity in _entityRegistry.MovableEntities)
            {
                _velocityUpdater.ApplyDrag(entity, deltaTime);
                _velocityUpdater.ApplyThresholds(entity);
                _velocityUpdater.UpdatePosition(entity, deltaTime);
                _velocityUpdater.UpdateRotation(entity, deltaTime);
            }

            // Handle collisions (detection + resolution)
            _collisionService.HandleCollisions(_worldService.CurrentMap);
        }
    }
}
