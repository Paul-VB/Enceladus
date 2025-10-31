using Enceladus.Core.Input;
using Enceladus.Core.Physics.Motion;
using System.Numerics;

namespace Enceladus.Core.Control.MotionControllers
{
    public interface IArrowKeysMotionController
    {
        void Update(IMovable entity, float deltaTime);
    }

    public class ArrowKeysMotionController : IArrowKeysMotionController
    {
        private readonly IInputReader _inputReader;
        private readonly IVelocityUpdater _velocityUpdater;
        private readonly float _thrust = 50f;

        public ArrowKeysMotionController(IInputReader inputReader, IVelocityUpdater velocityUpdater)
        {
            _inputReader = inputReader;
            _velocityUpdater = velocityUpdater;
        }

        public void Update(IMovable entity, float deltaTime)
        {
            var movementInput = _inputReader.GetArrowKeyMovementInput();
            if (movementInput != Vector2.Zero)
            {
                _velocityUpdater.ApplyForce(entity, movementInput * _thrust, deltaTime);
            }
        }
    }
}
