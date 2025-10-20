using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes.Helpers
{
    /// <summary>
    /// Decomposes concave polygons into a list of convex sub-polygons.
    /// SAT collision detection only works with convex shapes, so concave shapes
    /// must be broken down into convex pieces for collision checks.
    /// </summary>
    public interface IConcavePolygonSlicer
    {
        /// <summary>
        /// Slices a concave polygon into convex sub-polygons.
        /// Returns a list of vertex lists, where each inner list represents one convex polygon.
        /// </summary>
        List<List<Vector2>> Slice(List<Vector2> concavePolygon);
    }

    //todo: test this later, and probably replace it with a more performant algorithm. this is kinda literally the worst known algorith mfor this, but it will do for now
    public class EarClippingTriangulationSlicer : IConcavePolygonSlicer
    {
        public List<List<Vector2>> Slice(List<Vector2> concavePolygon)
        {
            if (concavePolygon.Count < 3)
                return new List<List<Vector2>>();

            var triangles = new List<List<Vector2>>();
            var remainingVertices = new List<Vector2>(concavePolygon);

            while (remainingVertices.Count > 3)
            {
                bool earFound = false;

                for (int i = 0; i < remainingVertices.Count; i++)
                {
                    int prevIndex = (i - 1 + remainingVertices.Count) % remainingVertices.Count;
                    int nextIndex = (i + 1) % remainingVertices.Count;

                    var prev = remainingVertices[prevIndex];
                    var current = remainingVertices[i];
                    var next = remainingVertices[nextIndex];

                    if (IsEar(prev, current, next, remainingVertices))
                    {
                        triangles.Add(new List<Vector2> { prev, current, next });
                        remainingVertices.RemoveAt(i);
                        earFound = true;
                        break;
                    }
                }

                if (!earFound)
                {
                    break;
                }
            }

            if (remainingVertices.Count == 3)
            {
                triangles.Add(remainingVertices);
            }

            return triangles;
        }

        private bool IsEar(Vector2 prev, Vector2 current, Vector2 next, List<Vector2> polygon)
        {
            if (!IsConvexVertex(prev, current, next))
                return false;

            for (int i = 0; i < polygon.Count; i++)
            {
                var point = polygon[i];
                if (point == prev || point == current || point == next)
                    continue;

                if (IsPointInTriangle(point, prev, current, next))
                    return false;
            }

            return true;
        }

        private bool IsConvexVertex(Vector2 prev, Vector2 current, Vector2 next)
        {
            var edge1 = current - prev;
            var edge2 = next - current;
            var cross = edge1.X * edge2.Y - edge1.Y * edge2.X;
            return cross > 0;
        }

        private bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            float sign1 = Sign(point, a, b);
            float sign2 = Sign(point, b, c);
            float sign3 = Sign(point, c, a);

            bool hasNeg = sign1 < 0 || sign2 < 0 || sign3 < 0;
            bool hasPos = sign1 > 0 || sign2 > 0 || sign3 > 0;

            return !(hasNeg && hasPos);
        }

        private float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }
    }
}
