using System.Numerics;

namespace Enceladus.Core.Physics.Collision.Detection
{
    public interface ICollisionInfoService
    {
        CollisionInfo GetDeepestCollision(List<CollisionInfo> collisions);
        CollisionInfo ReverseCollisionNormal(CollisionInfo collisionInfo);
    }

    public class CollisionInfoService : ICollisionInfoService
    {
        public CollisionInfo GetDeepestCollision(List<CollisionInfo> collisions)
        {
            var deepest = CollisionInfo.NonCollision;

            foreach (var collision in collisions)
            {
                if (collision.PenetrationDepth > deepest.PenetrationDepth)
                {
                    deepest = collision;
                }
            }

            return deepest;
        }

        public CollisionInfo ReverseCollisionNormal(CollisionInfo collisionInfo)
        {
            return new CollisionInfo
            {
                PenetrationDepth = collisionInfo.PenetrationDepth,
                CollisionNormal = -collisionInfo.CollisionNormal
            };
        }
    }
}
