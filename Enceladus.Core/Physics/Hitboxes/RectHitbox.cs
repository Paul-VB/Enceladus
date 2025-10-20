using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class RectHitbox : ConvexPolygonHitbox
    {
        //the 4 vertices in the base() correspond to the 4 corners of the rect in this order: top-left, top-right, bottom-right, bottom-left
        [SetsRequiredMembers]
        public RectHitbox(Vector2 size) : base([-size / 2, new(size.X / 2, -size.Y / 2), size / 2, new(-size.X / 2, size.Y / 2)])
        {
            Size = size;
        }

        public Vector2 Size { get; set; }
    }
}