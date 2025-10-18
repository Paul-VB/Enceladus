using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollidable
    {
        Vector2 Position { get; }
        float Rotation { get; }
        Hitbox Hitbox { get; set; }
    }
}
