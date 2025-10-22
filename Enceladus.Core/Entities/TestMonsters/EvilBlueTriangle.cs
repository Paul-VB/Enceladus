using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Entities.TestMonsters
{
    public class EvilBlueTriangle : MovableEntity
    {
        public override IHitbox Hitbox { get; set; }

        public EvilBlueTriangle()
        {

            // 3x3 unit triangle - equilateral triangle centered at origin
            var vertices = new List<Vector2>
            {
                new(0f, -1.5f),      // Top point
                new(-1.5f, 1.5f),    // Bottom left
                new(1.5f, 1.5f)      // Bottom right
            };

            Hitbox = new ConvexPolygonHitbox(vertices);
        }

        public override void Draw(Camera2D camera)
        {
            // Draw triangle using the hitbox vertices
            var polygonHitbox = (PolygonHitbox)Hitbox;

            // Transform vertices to world space
            var worldVertices = GeometryHelper.TransformToWorldSpace(polygonHitbox.Vertices, Position, Rotation);

            // Draw the triangle
            Raylib.DrawTriangle(worldVertices[0], worldVertices[1], worldVertices[2], Color.Blue);

            // Draw outline
            Raylib.DrawTriangleLines(worldVertices[0], worldVertices[1], worldVertices[2], Color.DarkBlue);
        }
    }
}
