using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Utils
{
    public static class GeometryHelper
    {
        /// <summary>
        /// Transforms a list of local vertices to world space by applying rotation and translation.
        /// </summary>
        public static List<Vector2> TransformToWorldSpace(List<Vector2> localVertices, Vector2 position, float rotationDegrees)
        {
            float radians = AngleHelper.DegToRad(rotationDegrees);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            var worldVertices = new List<Vector2>();

            foreach (var vertex in localVertices)
            {
                // Rotate vertex
                var rotated = new Vector2(
                    vertex.X * cos - vertex.Y * sin,
                    vertex.X * sin + vertex.Y * cos
                );

                // Translate to world position
                worldVertices.Add(rotated + position);
            }

            return worldVertices;
        }


        /// <summary>
        /// Returns the bounding rectangle for a cell at the given world coordinates.
        /// Cells are 1x1 squares in world space.
        /// </summary>
        //todo: do we need this anymore?
        public static Rectangle GetCellBounds(int cellX, int cellY)
        {
            return new Rectangle(cellX, cellY, 1, 1);
        }

        /// <summary>
        /// Creates a Rectangle from minimum and maximum bounds.
        /// Calculates width and height from the min/max values.
        /// </summary>
        public static Rectangle RectangleFromBounds(float minX, float maxX, float minY, float maxY)
        {
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Checks if two rectangles overlap.
        /// Returns true if the rectangles intersect, false otherwise.
        /// </summary>
        public static bool DoRectanglesOverlap(Rectangle rect1, Rectangle rect2)
        {
            // Standard AABB (Axis-Aligned Bounding Box) overlap test
            return rect1.X < rect2.X + rect2.Width &&
                   rect1.X + rect1.Width > rect2.X &&
                   rect1.Y < rect2.Y + rect2.Height &&
                   rect1.Y + rect1.Height > rect2.Y;
        }

        /// <summary>
        /// Determines if a polygon is convex by checking the cross product sign consistency.
        /// Uses the cross product of consecutive edge vectors - all must have the same sign for convexity.
        /// </summary>
        /// <param name="vertices">The vertices of the polygon in order (clockwise or counter-clockwise)</param>
        /// <returns>True if the polygon is convex, false if concave or degenerate</returns>
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
                if (MathF.Abs(crossProduct) < 1e-6f) //todo: magic number maybe
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
