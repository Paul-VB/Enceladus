using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface IAabbCalculator
    {
        AabbRectangle CalculateAabb(Entity entity);
    }

    public class AabbCalculator : IAabbCalculator
    {
        public AabbRectangle CalculateAabb(Entity entity)
        {
            if (entity.Hitbox is RectHitbox rect)
                return CalculateAabbFromRect(rect, entity.Position, entity.Rotation);

            if (entity.Hitbox is CircleHitbox circle)
                return CalculateAabbFromCircle(circle, entity.Position);

            if (entity.Hitbox is PolygonHitbox polygon)
                return CalculateAabbFromPolygon(polygon, entity.Position, entity.Rotation);

            throw new NotSupportedException($"Hitbox type not supported: {entity.Hitbox?.GetType()}");
        }

        private AabbRectangle CalculateAabbFromRect(RectHitbox rectHitbox, Vector2 position, float rotation)
        {
            // Use your derived formula: AABB = |W*cos(θ)| + |H*sin(θ)|
            float radians = rotation * (MathF.PI / 180f);
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

            return new AabbRectangle(rectangle);
        }

        private AabbRectangle CalculateAabbFromCircle(CircleHitbox circleHitbox, Vector2 position)
        {
            float diameter = circleHitbox.Radius * 2f;

            var rectangle = new Rectangle(
                position.X - circleHitbox.Radius,
                position.Y - circleHitbox.Radius,
                diameter,
                diameter
            );

            return new AabbRectangle(rectangle);
        }

        private AabbRectangle CalculateAabbFromPolygon(PolygonHitbox polygonHitbox, Vector2 position, float rotation)
        {
            // TODO: Rotate vertices and find min/max X and Y
            // 1. Convert rotation to radians
            // 2. Rotate each vertex around origin
            // 3. Find min/max X and Y of rotated vertices
            // 4. Offset by entity position
            throw new NotImplementedException();
        }
    }

    public class AabbRectangle
    {
        public AabbRectangle()
        {
            
        }
        public AabbRectangle(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }
        public Rectangle Rectangle { get; set; }
        public int MinX => (int)MathF.Floor(Rectangle.X);
        public int MaxX => (int)MathF.Ceiling(Rectangle.X + Rectangle.Width);
        public int MinY => (int)MathF.Floor(Rectangle.Y);
        public int MaxY => (int)MathF.Ceiling(Rectangle.Y + Rectangle.Height);
    }
}
