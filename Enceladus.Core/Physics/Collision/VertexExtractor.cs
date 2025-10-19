using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using Enceladus.Core.World;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface IVertexExtractor
    {
        List<List<Vector2>> ExtractWorldVerticeses(ICollidable collidable);
    }

    public class VertexExtractor : IVertexExtractor
    {

        //Verticeses is not a typo. if vertex is singular, and vertices is plural, then vertices*es* is plural squared
        public List<List<Vector2>> ExtractWorldVerticeses(ICollidable collidable)
        {
            if (collidable is Cell cell) return GetCellVertices(cell).AsList();

            // Get local vertices based on hitbox type
            List<List<Vector2>> localVerticeses = collidable.Hitbox switch
            {
                RectHitbox rect => GetRectVertices(rect).AsList(),
                PolygonHitbox poly => poly.Vertices.AsList(),
                ConcavePolygonHitbox concavePoly => concavePoly.ConvexSlices.Select(x => x.Vertices).ToList(),
                CircleHitbox => throw new NotSupportedException("Circle hitboxes don't have vertices - use circle collision detector"),
                _ => throw new NotSupportedException($"Hitbox type not supported: {collidable.Hitbox?.GetType()}")
            };

            // Transform to world space
            var WorldVerticeses = localVerticeses.Select(x => GeometryHelper.TransformToWorldSpace(x, collidable.Position, collidable.Rotation)).ToList();
            return WorldVerticeses;
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
    }
}
