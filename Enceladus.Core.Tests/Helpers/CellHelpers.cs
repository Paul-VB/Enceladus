using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using System.Numerics;

namespace Enceladus.Core.Tests.Helpers
{
    /// <summary>
    /// Factory methods for creating test cells
    /// </summary>
    public static class CellHelpers
    {
        /// <summary>
        /// Creates a test cell with pre-computed hitbox vertices
        /// </summary>
        public static Cell CreateTestCell(int x, int y, bool hasCollision = true)
        {
            var vertices = new List<Vector2>
            {
                new(x, y),           // top-left
                new(x + 1, y),       // top-right
                new(x + 1, y + 1),   // bottom-right
                new(x, y + 1)        // bottom-left
            };

            return new Cell
            {
                X = x,
                Y = y,
                CellType = new CellType { HasCollision = hasCollision },
                Hitbox = new CellHitbox(vertices)
            };
        }
    }
}
