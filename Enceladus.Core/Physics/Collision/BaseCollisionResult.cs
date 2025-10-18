using Enceladus.Core.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public class BaseCollisionResult
    {
        public required MoveableEntity Entity { get; set; }
        public required ICollidable OtherObject { get; set; }
        public required float PenetrationDepth { get; set; }
        public required Vector2 CollisionNormal { get; set; }
    }
}
