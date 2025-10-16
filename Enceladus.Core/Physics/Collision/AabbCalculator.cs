using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface IAabbCalculator
    {
        Rectangle CalculateAabb(ICollidableEntity entity);
    }

    public class AabbCalculator : IAabbCalculator
    {
        public Rectangle CalculateAabb(ICollidableEntity entity)
        {
            if (entity.Hitbox is RectHitbox rect)
                return CalculateAabbFromRect(rect, entity.Position, entity.Rotation);

            if (entity.Hitbox is CircleHitbox circle)
                return CalculateAabbFromCircle(circle, entity.Position);

            if (entity.Hitbox is PolygonHitbox polygon)
                return CalculateAabbFromPolygon(polygon, entity.Position, entity.Rotation);

            throw new NotSupportedException($"Hitbox type not supported: {entity.Hitbox?.GetType()}");
        }

        private Rectangle CalculateAabbFromRect(RectHitbox rectHitbox, Vector2 position, float rotation)
        {
            // Use your derived formula: AABB = |W*cos(θ)| + |H*sin(θ)|
            float radians = AngleHelper.DegToRad(rotation);
            float cos = MathF.Abs(MathF.Cos(radians));
            float sin = MathF.Abs(MathF.Sin(radians));

            float aabbWidth = rectHitbox.Size.X * cos + rectHitbox.Size.Y * sin;
            float aabbHeight = rectHitbox.Size.X * sin + rectHitbox.Size.Y * cos;

            var rectangle = new Rectangle(
                    position.X - aabbWidth / 2f,
                    position.Y - aabbHeight / 2f,
                    aabbWidth,
                    aabbHeight
                );

            return rectangle;
        }

        private Rectangle CalculateAabbFromCircle(CircleHitbox circleHitbox, Vector2 position)
        {
            float diameter = circleHitbox.Radius * 2f;

            var rectangle = new Rectangle(
                position.X - circleHitbox.Radius,
                position.Y - circleHitbox.Radius,
                diameter,
                diameter
            );

            return rectangle;
        }

        private Rectangle CalculateAabbFromPolygon(PolygonHitbox polygonHitbox, Vector2 position, float rotation)
        {
            // Rotate vertices and find min/max bounds
            float radians = AngleHelper.DegToRad(rotation);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (var vertex in polygonHitbox.Vertices)
            {
                // Rotate vertex around origin
                float rotatedX = vertex.X * cos - vertex.Y * sin;
                float rotatedY = vertex.X * sin + vertex.Y * cos;

                // Add entity position to get world coordinates
                float worldX = rotatedX + position.X;
                float worldY = rotatedY + position.Y;

                // Track min/max
                minX = MathF.Min(minX, worldX);
                maxX = MathF.Max(maxX, worldX);
                minY = MathF.Min(minY, worldY);
                maxY = MathF.Max(maxY, worldY);
            }
            var rectangle = GeometryHelper.RectangleFromBounds(minX, maxX, minY, maxY);

            return rectangle;
        }
    }
}
