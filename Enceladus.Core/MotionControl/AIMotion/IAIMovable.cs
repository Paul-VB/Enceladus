using Enceladus.Core.Physics.Motion;

namespace Enceladus.Core.MotionControl.AIMotion
{
    public interface IAIMovable : IMovable
    {
        AIMotionControllerType MotionControllerType { get; }
    }
}
