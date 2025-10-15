using Enceladus.Core.Physics.Hitboxes;
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
        void Draw(Camera2D camera);
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
        public virtual void Draw(Camera2D camera)
        {
            var size = new Vector2(Sprite.Width / camera.Zoom, Sprite.Height / camera.Zoom);
            var origin = size / 2f;
            var source = new Rectangle(0, 0, Sprite.Width, Sprite.Height);
            var dest = new Rectangle(Position, size);

            Raylib.DrawTexturePro(Sprite, source, dest, origin, Rotation, Color.White);
        }
    }
}
