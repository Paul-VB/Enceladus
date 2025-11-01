using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Motion;
using Enceladus.Core.MotionControl.PlayerMotion;

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
            foreach (var movable in _entityRegistry.PlayerMovableEntities)
            {
                ApplyPlayerMotionController(movable, deltaTime);
            }

            // AI-controlled entities would be handled here in the future
            // foreach (var movable in _entityRegistry.AIMovableEntities)
            // {
            //     ApplyAIMotionController(movable, deltaTime);
            // }
        }

        private void ApplyPlayerMotionController(IPlayerMovable movable, float deltaTime)
        {
            switch (movable.MotionControllerType)
            {
                case PlayerMotionControllerType.PlayerInput:
                    _playerInputController.Update((Player)movable, deltaTime);
                    break;
                case PlayerMotionControllerType.ArrowKeys:
                    _arrowKeysMotionController.Update(movable, deltaTime);
                    break;
            }
        }
    }
}
