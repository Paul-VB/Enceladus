using Raylib_cs;
using System.Numerics;

namespace Enceladus.Entities
{
    public interface IEntity
    {
        Guid Guid { get; set; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Size { get; set; }
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
        public Vector2 Size { get; set; } = Vector2.One;
        public abstract void Update(float deltaTime);
        public virtual void Draw()
        {
            var origin = Size / 2f;
            var source = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            var dest = new Rectangle(Position, Size);

            Raylib.DrawTexturePro(Sprite, source, dest, origin, Rotation, Color.White);
        }
    }
}
