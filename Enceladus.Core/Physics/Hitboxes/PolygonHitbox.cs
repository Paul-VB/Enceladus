using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class PolygonHitbox : Hitbox
    {
        public required List<Vector2> Vertices {  get; set; }
    }
}
