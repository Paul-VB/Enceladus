using Enceladus.Core.Control.MotionControllers;
using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Motion;

namespace Enceladus.Core.Control
{
    public interface IMotionControlService
    {
        void UpdateAll(float deltaTime);
    }

    public class MotionControlService : IMotionControlService
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IPlayerInputController _playerInputController;
        private readonly IArrowKeysMotionController _arrowKeysMotionController;

        public MotionControlService(
            IEntityRegistry entityRegistry,
            IPlayerInputController playerInputController,
            IArrowKeysMotionController arrowKeysMotionController)
        {
            _entityRegistry = entityRegistry;
            _playerInputController = playerInputController;
            _arrowKeysMotionController = arrowKeysMotionController;
        }

        public void UpdateAll(float deltaTime)
        {
            foreach (var entity in _entityRegistry.MovableEntities)
            {
                ApplyMotionController(entity, deltaTime);
            }
        }

        private void ApplyMotionController(IMovable entity, float deltaTime)
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
    }
}
