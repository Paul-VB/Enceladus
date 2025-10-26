using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using Enceladus.Core.World;
using System.Numerics;

namespace Enceladus.Core.Tests.Helpers
{
    /// <summary>
    /// Factory methods for creating test cells
    /// </summary>
    public static class CellHelpers
    {
        public static Cell GivenACell(int x, int y, bool hasCollision = true)
        {
            var vertices = new List<Vector2>
            {
                new(x, y),           // top-left
                new(x + 1, y),       // top-right
                new(x + 1, y + 1),   // bottom-right
                new(x, y + 1)        // bottom-left
            };

            var cell = new Cell
            {
                X = x,
                Y = y,
                CellType = new CellType
                {
                    Id = -1,
                    HasCollision = hasCollision,
                },
                Hitbox = new CellHitbox(vertices)
            };

            return cell;
        }
    }
}
