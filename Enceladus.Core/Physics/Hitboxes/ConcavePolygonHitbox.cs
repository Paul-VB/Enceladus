using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class ConcavePolygonHitbox : PolygonHitbox
    {
        public ConcavePolygonHitbox()
        {
            
        }
        [SetsRequiredMembers]
        public ConcavePolygonHitbox(List<Vector2> vertices) : base(vertices)
        {
        }
        public required List<ConvexPolygonHitbox> ConvexSlices { get; set; } = [];     

    }
}
