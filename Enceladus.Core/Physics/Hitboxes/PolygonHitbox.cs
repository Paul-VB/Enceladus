using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class PolygonHitbox : IHitbox
    {
        public PolygonHitbox(List<Vector2> vertices)
        {
            Vertices = vertices;
        }

        public List<Vector2> Vertices {  get; set; }
    }

}
