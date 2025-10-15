using Enceladus.Core.Physics.Hitboxes;

namespace Enceladus.Entities
{
    public interface ICollidableEntity : IEntity
    {
        Hitbox Hitbox { get; set; }
    }

    public abstract class CollidableEntity : Entity, ICollidableEntity
    {
        public abstract Hitbox Hitbox { get; set; }
    }
}
