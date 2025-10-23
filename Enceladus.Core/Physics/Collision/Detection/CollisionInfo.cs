using System.Numerics;

namespace Enceladus.Core.Physics.Collision.Detection
{
    public class CollisionInfo
    {
        public static CollisionInfo NonCollision = new()
        {
            PenetrationDepth = 0,
            CollisionNormal = Vector2.Zero
        };

        public float PenetrationDepth { get; init; }
        public Vector2 CollisionNormal { get; init; }
    }
}
