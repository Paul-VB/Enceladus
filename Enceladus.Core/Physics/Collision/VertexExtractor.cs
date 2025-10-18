using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface IVertexExtractor
    {
        List<Vector2> ExtractWorldVertices(ICollidable collidable);
    }

    public class VertexExtractor : IVertexExtractor
    {
        public List<Vector2> ExtractWorldVertices(ICollidable collidable)
        {
            if (collidable is Cell cell) return GetCellVertices(cell);

            // Get local vertices based on hitbox type
            List<Vector2> localVertices = collidable.Hitbox switch
            {
                RectHitbox rect => GetRectVertices(rect),
                PolygonHitbox poly => poly.Vertices,
                CircleHitbox => throw new NotSupportedException("Circle hitboxes don't have vertices - use circle collision detector"),
                _ => throw new NotSupportedException($"Hitbox type not supported: {collidable.Hitbox?.GetType()}")
            };

            // Transform to world space
            return TransformToWorldSpace(localVertices, collidable.Position, collidable.Rotation);
        }

        private List<Vector2> GetCellVertices(Cell cell)
        {
            // Cell is a 1x1 square, no rotation
            float x = cell.X;
            float y = cell.Y;

            return new List<Vector2>
            {
                new Vector2(x, y),           // top-left
                new Vector2(x + 1, y),       // top-right
                new Vector2(x + 1, y + 1),   // bottom-right
                new Vector2(x, y + 1)        // bottom-left
            };
        }

        private List<Vector2> GetRectVertices(RectHitbox rect)
        {
            float halfWidth = rect.Size.X / 2f;
            float halfHeight = rect.Size.Y / 2f;

            return new List<Vector2>
            {
                new Vector2(-halfWidth, -halfHeight),  // top-left
                new Vector2(halfWidth, -halfHeight),   // top-right
                new Vector2(halfWidth, halfHeight),    // bottom-right
                new Vector2(-halfWidth, halfHeight)    // bottom-left
            };
        }

        private List<Vector2> TransformToWorldSpace(List<Vector2> localVertices, Vector2 position, float rotation)
        {
            float radians = AngleHelper.DegToRad(rotation);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            var worldVertices = new List<Vector2>();

            foreach (var vertex in localVertices)
            {
                // Rotate vertex
                //todo: is this the only place we rotate vertecies? should we pull this out?
                var rotated = new Vector2(
                    vertex.X * cos - vertex.Y * sin,
                    vertex.X * sin + vertex.Y * cos
                );

                // Translate to world position
                worldVertices.Add(rotated + position);
            }

            return worldVertices;
        }
    }
}
