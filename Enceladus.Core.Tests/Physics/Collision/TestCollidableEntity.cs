using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Tests.Physics.Collision
{
    /// <summary>
    /// Test helper entity for collision testing
    /// </summary>
    internal class TestCollidableEntity : ICollidableEntity
    {
        public TestCollidableEntity(Hitbox hitbox, Vector2 position, float rotation)
        {
            Hitbox = hitbox;
            Position = position;
            Rotation = rotation;
            Guid = Guid.NewGuid();
        }

        public Hitbox Hitbox { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Guid Guid { get; set; }
        public Texture2D Sprite { get; set; }

        public void Draw(Camera2D camera) { }
        public void Update(float deltaTime) { }
    }
}
