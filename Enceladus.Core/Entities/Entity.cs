using System.Numerics;

namespace Enceladus.Core.Entities
{

    public abstract class Entity
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
    }
}
