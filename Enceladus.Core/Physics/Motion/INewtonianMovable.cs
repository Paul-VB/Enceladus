using System.Numerics;

namespace Enceladus.Core.Physics.Motion
{
    //both Movabe and Moveable are technically correct spelling. dont fret
    public interface INewtonianMovable : IMovable
    {
        float Mass { get; set; }
        float Drag { get; set; }
        float AngularDrag { get; set; }
        float MinVelocityThreshold { get; set; }
        float MinAngularVelocityThreshold { get; set; }

        void Accelerate(Vector2 force, float deltaTime);
        void ApplyTorque(float torque, float deltaTime);
    }
}
