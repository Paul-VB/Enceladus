using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class RectHitbox : Hitbox
    {
        public RectHitbox(Vector2 size)
        {
            Size = size;
        }

        public RectHitbox(float width, float height)
        {
            Size = new Vector2(width, height);
        }
        public Vector2 Size { get; set; }
    }
}