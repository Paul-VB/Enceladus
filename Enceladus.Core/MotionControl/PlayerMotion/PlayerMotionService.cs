using Enceladus.Core.Entities;

namespace Enceladus.Core.MotionControl.PlayerMotion
{
    public interface IPlayerMotionService
    {
        void Update(float deltaTime);
    }

    public class PlayerMotionService : IPlayerMotionService
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IPlayerInputController _playerInputController;
        private readonly IArrowKeysMotionController _arrowKeysMotionController;

        public PlayerMotionService(
            IEntityRegistry entityRegistry,
            IPlayerInputController playerInputController,
            IArrowKeysMotionController arrowKeysMotionController)
        {
            _entityRegistry = entityRegistry;
            _playerInputController = playerInputController;
            _arrowKeysMotionController = arrowKeysMotionController;
        }

        public void Update(float deltaTime)
        {
            foreach (var movable in _entityRegistry.PlayerMovableEntities)
            {
                ApplyMotionController(movable, deltaTime);
            }
        }

        private void ApplyMotionController(IPlayerMovable movable, float deltaTime)
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
