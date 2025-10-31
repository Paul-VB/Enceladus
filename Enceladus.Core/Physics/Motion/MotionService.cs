using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Motion.MotionControllers;

namespace Enceladus.Core.Physics.Motion
{
    public interface IMotionService
    {
        void UpdateAll(float deltaTime);
    }

    public class MotionService : IMotionService
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IVelocityUpdater _velocityUpdater;
        private readonly IPlayerInputController _playerInputController;
        private readonly IArrowKeysMotionController _arrowKeysMotionController;

        public MotionService(
            IEntityRegistry entityRegistry,
            IVelocityUpdater velocityUpdater,
            IPlayerInputController playerInputController,
            IArrowKeysMotionController arrowKeysMotionController)
        {
            _entityRegistry = entityRegistry;
            _velocityUpdater = velocityUpdater;
            _playerInputController = playerInputController;
            _arrowKeysMotionController = arrowKeysMotionController;
        }

        public void UpdateAll(float deltaTime)
        {
            // Phase 1: Motion controllers apply forces (decision phase)
            foreach (var entity in _entityRegistry.MovableEntities)
            {
                HandleMotionControl(entity, deltaTime);
                UpdateVelocity(entity, deltaTime);
            }
        }

        private void HandleMotionControl(IMovable entity, float deltaTime)
        {
            switch (entity.MotionControllerType)
            {
                case MotionControllerType.PlayerInput:
                    _playerInputController.Update((Player)entity, deltaTime);
                    break;
                case MotionControllerType.ArrowKeys:
                    _arrowKeysMotionController.Update(entity, deltaTime);
                    break;
                case MotionControllerType.None:
                    // No controller logic
                    break;
            }
        }

        private void UpdateVelocity(IMovable entity, float deltaTime)
        {
            _velocityUpdater.ApplyDrag(entity, deltaTime);
            _velocityUpdater.ApplyThresholds(entity);
            _velocityUpdater.UpdatePosition(entity, deltaTime);
            _velocityUpdater.UpdateRotation(entity, deltaTime);
        }
    }
}
