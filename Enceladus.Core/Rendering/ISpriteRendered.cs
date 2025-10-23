using System.Numerics;

namespace Enceladus.Core.Rendering
{
    public interface ISpriteRendered
    {
        SpriteDefinition CurrentSprite { get; set; }
        SpriteModifiers SpriteModifiers { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
    }
}
