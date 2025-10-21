using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class CellHitbox : IHitbox
    {
        public List<Vector2> PretransformedVertices { get; }

        public CellHitbox(List<Vector2> vertices)
        {
            PretransformedVertices = vertices;
        }
    }
}
