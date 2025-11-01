using Enceladus.Core.Physics.Motion;

namespace Enceladus.Core.MotionControl.PlayerMotion
{
    public interface IPlayerMovable : IMovable
    {
        PlayerMotionControllerType MotionControllerType { get; }
    }
}
