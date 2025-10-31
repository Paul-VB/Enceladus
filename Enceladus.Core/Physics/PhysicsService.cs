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
        private readonly IWorldService _worldService;
        private readonly ICollisionService _collisionService;
        private readonly IMotionService _motionService;

        public PhysicsService(IWorldService worldService, ICollisionService collisionService, IMotionService motionService)
        {
            _worldService = worldService;
            _collisionService = collisionService;
            _motionService = motionService;
        }

        public void Update(float deltaTime)
        {
            // Update all entities (movement, rotation, etc.)
            _motionService.UpdateAll(deltaTime);

            // Handle collisions (detection + resolution)
            _collisionService.HandleCollisions(_worldService.CurrentMap);
        }
    }
}
