using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public abstract class PolygonHitbox : IHitbox
    {
        protected PolygonHitbox()
        {
            
        }

        [SetsRequiredMembers]
        protected PolygonHitbox(List<Vector2> vertices)
        {
            Vertices = vertices;
        }

        public virtual required List<Vector2> Vertices { get; set; } = [];
    }

}
