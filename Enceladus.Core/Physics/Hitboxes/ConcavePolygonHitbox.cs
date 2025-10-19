using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class ConcavePolygonHitbox : IHitbox
    {
        public required List<Vector2> OuterVertices { get; set; }
        public required List<PolygonHitbox> ConvexSlices { get; set; }        

    }
}
