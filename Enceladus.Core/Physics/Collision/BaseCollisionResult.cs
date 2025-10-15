using Enceladus.Core.World;
using Enceladus.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public class BaseCollisionResult
    {
        public required ICollidableEntity Entity { get; set; }
        public required float PenetrationDepth { get; set; }
        public required Vector2 CollisionNormal { get; set; }
    }

    public class EntityToEntityCollisionResult : BaseCollisionResult
    {
        public required ICollidableEntity OtherEntity { get; set; }

    }

    public class EntityToCellCollisionResult : BaseCollisionResult
    {
        public required Cell Cell { get; set; }
    }

}
