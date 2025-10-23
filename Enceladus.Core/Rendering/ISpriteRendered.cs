using System.Numerics;

namespace Enceladus.Core.Rendering
{
    public interface ISpriteRendered
    {
        SpriteDefinition CurrentSprite { get; }
        SpriteModifiers SpriteModifiers { get; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
    }
}
