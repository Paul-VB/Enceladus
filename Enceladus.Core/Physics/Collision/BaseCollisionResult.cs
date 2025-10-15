using Enceladus.Core.World;
using Enceladus.Entities;

namespace Enceladus.Core.Physics.Collision
{
    public class BaseCollisionResult
    {
        public required Entity Entity { get; set; }
    }

    public class EntityToEntityCollisionResult : BaseCollisionResult
    {
        public required Entity OtherEntity { get; set; }

    }

    public class EntityToCellCollisionResult : BaseCollisionResult
    {
        public required Cell Cell { get; set; }
    }

}
