using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Utils
{
    public static class GeometryHelper
    {
        private const float NEARLY_ZERO_THRESHOLD = 1e-6f;

        public static List<Vector2> TransformToWorldSpace(List<Vector2> localVertices, Vector2 position, float rotationDegrees)
        {
            float radians = AngleHelper.DegToRad(rotationDegrees);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            var worldVertices = new List<Vector2>();

            foreach (var localVertex in localVertices)
            {
                worldVertices.Add(TransformToWorldSpace(localVertex, position, cos, sin));
            }

            return worldVertices;
        }

        public static Vector2 TransformToWorldSpace(Vector2 localVertex, Vector2 position, float rotationDegrees)
        {
            float radians = AngleHelper.DegToRad(rotationDegrees);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            
            return TransformToWorldSpace(localVertex, position, cos, sin);
        }

        private static Vector2 TransformToWorldSpace(Vector2 localVertex, Vector2 position, float cosineRadians, float sineRadians)
        {
            var rotated = new Vector2(
                localVertex.X * cosineRadians - localVertex.Y * sineRadians,
                localVertex.X * sineRadians + localVertex.Y * cosineRadians
            );

            // Translate to world position
            return rotated + position;
        }



        /// <summary>
        /// Returns the bounding rectangle for a cell at the given world coordinates.
        /// Cells are 1x1 squares in world space.
        /// Hot path optimization - this is called per-frame for collision detection.
        /// </summary>
        public static Rectangle GetCellBounds(int cellX, int cellY)
        {
            return new Rectangle(cellX, cellY, 1, 1);
        }

        public static Rectangle RectangleFromBounds(float minX, float maxX, float minY, float maxY)
        {
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public static bool DoRectanglesOverlap(Rectangle rect1, Rectangle rect2)
        {
            return rect1.X < rect2.X + rect2.Width &&
                   rect1.X + rect1.Width > rect2.X &&
                   rect1.Y < rect2.Y + rect2.Height &&
                   rect1.Y + rect1.Height > rect2.Y;
        }

        /// <summary>
        /// Normalizes a vector using a precomputed distance/length.
        /// Avoids redundant square root calculation when distance is already known.
        /// Returns the same result as Vector2.Normalize(), but it just saves CPU cycles if you happen to already have the vector's magnitude
        /// </summary>
        public static Vector2 NormalizeByDistance(Vector2 vector, float distance)
        {
            if (distance > 0)
                return vector / distance;

            // Fallback for zero-length vector (prevents division by zero)
            // Returns a default direction (downward)
            return new Vector2(0, -1);
        }

        public static bool IsConvex(List<Vector2> vertices)
        {
            if (vertices == null || vertices.Count < 3)
                return false; // Degenerate polygon

            if (vertices.Count == 3)
                return true;

            int n = vertices.Count;
            bool? isPositive = null; // Track the expected sign of cross products

            for (int i = 0; i < n; i++)
            {
                // Get three consecutive vertices (wrapping around)
                Vector2 v1 = vertices[i];
                Vector2 v2 = vertices[(i + 1) % n];
                Vector2 v3 = vertices[(i + 2) % n];

                // Calculate edge vectors
                Vector2 edge1 = v2 - v1;
                Vector2 edge2 = v3 - v2;

                // Calculate 2D cross product (z-component of 3D cross product)
                float crossProduct = edge1.X * edge2.Y - edge1.Y * edge2.X;

                // Skip collinear points (cross product near zero)
                if (MathF.Abs(crossProduct) < NEARLY_ZERO_THRESHOLD)
                    continue;

                // Check if this cross product matches the established sign
                bool currentIsPositive = crossProduct > 0;

                if (isPositive == null)
                {
                    // First non-zero cross product establishes the expected sign
                    isPositive = currentIsPositive;
                }
                else if (isPositive != currentIsPositive)
                {
                    // Sign changed - polygon is concave
                    return false;
                }
            }

            // All cross products had consistent sign (or all were collinear) - convex
            return true;
        }
    }
}
