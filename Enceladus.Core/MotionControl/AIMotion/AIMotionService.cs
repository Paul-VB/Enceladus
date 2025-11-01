using Enceladus.Core.Entities;

namespace Enceladus.Core.MotionControl.AIMotion
{
    public interface IAIMotionService
    {
        void Update(float deltaTime);
    }

    public class AIMotionService : IAIMotionService
    {
        private readonly IEntityRegistry _entityRegistry;

        public AIMotionService(IEntityRegistry entityRegistry)
        {
            _entityRegistry = entityRegistry;
        }

        public void Update(float deltaTime)
        {
            // Future: Handle AI-movable entities
            // foreach (var movable in _entityRegistry.AIMovableEntities)
            // {
            //     ApplyMotionController(movable, deltaTime);
            // }
        }
    }
}
