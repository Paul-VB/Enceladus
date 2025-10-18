using System.Numerics;

namespace Enceladus.Core.Physics.Motion
{
    //both Movabe and Moveable are technically correct spelling. dont fret
    public interface IMovable
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        float Mass { get; set; }
        Vector2 Velocity { get; set; }
        float Drag { get; set; }
        float AngularVelocity { get; set; }
        float AngularDrag { get; set; }
        void Accelerate(Vector2 force, float deltaTime);
        void ApplyTorque(float torque, float deltaTime);
    }
}
