using Raylib_cs;
using System.Numerics;

namespace Enceladus.Entities
{
    public interface IEntity
    {
        Guid Guid { get; set; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Texture2D Sprite { get; set; }
        void Update(float deltaTime);
        void Draw();
    }
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            
        }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Texture2D Sprite { get; set; }
        public abstract void Update(float deltaTime);
        public abstract void Draw();
    }
}
