using System.Diagnostics.CodeAnalysis;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class CellHitbox : ConvexPolygonHitbox
    {
        [SetsRequiredMembers]
        public CellHitbox() : base([new(0, 0), new(1, 0), new(1, 1), new(0, 1)])
        {
        }
    }
}
