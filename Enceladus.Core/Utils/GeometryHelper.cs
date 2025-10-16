using Raylib_cs;

namespace Enceladus.Core.Utils
{
    public static class GeometryHelper
    {
        /// <summary>
        /// Returns the bounding rectangle for a cell at the given world coordinates.
        /// Cells are 1x1 squares in world space.
        /// </summary>
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
    }
}
