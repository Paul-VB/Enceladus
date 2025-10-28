using System.Numerics;

namespace Enceladus.Core.Physics.Motion
{
    public interface IMovable
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Velocity { get; set; }
        float AngularVelocity { get; set; }
    }
}
