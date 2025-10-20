using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class ConvexPolygonHitbox : PolygonHitbox
    {
        [SetsRequiredMembers]
        public ConvexPolygonHitbox(List<Vector2> vertices) : base(vertices)
        {
        }
    }
}
