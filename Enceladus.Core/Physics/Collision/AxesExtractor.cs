using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface IAxesExtractor
    {
        List<Vector2> ExtractAxes(List<Vector2> vertices, IHitbox hitbox);
    }

    public class AxesExtractor : IAxesExtractor
    {
        public List<Vector2> ExtractAxes(List<Vector2> vertices, IHitbox hitbox)
        {
            if (hitbox is RectHitbox)
                return GetRectAxes(vertices);

            if (hitbox is PolygonHitbox)
                return GetPolygonAxes(vertices);

            throw new NotSupportedException($"Hitbox type not supported for SAT: {hitbox?.GetType()}");
        }

        private List<Vector2> GetRectAxes(List<Vector2> vertices)
        {
            // Rectangles have 4 edges, but opposite edges are parallel
            // Only need 2 unique axes (perpendicular to first two edges)
            var axes = new List<Vector2>();

            // Edge 0-1
            var edge1 = vertices[1] - vertices[0];
            axes.Add(GetPerpendicularAxis(edge1));

            // Edge 1-2
            var edge2 = vertices[2] - vertices[1];
            axes.Add(GetPerpendicularAxis(edge2));

            // Edges 2-3 and 3-0 are parallel to these, so we skip them
            return axes;
        }

        private List<Vector2> GetPolygonAxes(List<Vector2> vertices)
        {
            //todo: verify this gets the perpendicular axis between the final vertex and the firts vertex. it seems it does not do this at first glance. we need a test for this!
            // Get perpendicular axis for each edge
            var axes = new List<Vector2>();

            for (int i = 0; i < vertices.Count; i++)
            {
                int nextIndex = (i + 1) % vertices.Count;
                var edge = vertices[nextIndex] - vertices[i];
                axes.Add(GetPerpendicularAxis(edge));
            }

            return axes;
        }

        private Vector2 GetPerpendicularAxis(Vector2 edge)
        {
            // Get perpendicular vector (normal) and normalize it
            var perpendicular = new Vector2(-edge.Y, edge.X);
            return Vector2.Normalize(perpendicular);
        }
    }
}
