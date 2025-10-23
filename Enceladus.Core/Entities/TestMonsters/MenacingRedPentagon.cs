using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using Enceladus.Core.Utils;
using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Entities.TestMonsters
{
    public class MenacingRedPentagon : MovableEntity, ICollidable, IGeometryRendered
    {
        public override IHitbox Hitbox { get; set; }

        public MenacingRedPentagon()
        {
            // 3x3 unit pentagon - regular pentagon centered at origin
            var vertices = new List<Vector2>();
            float radius = 1.5f;

            for (int i = 0; i < 5; i++)
            {
                float angle = AngleHelper.DegToRad(i * 72f - 90f); // Start from top
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                vertices.Add(new Vector2(x, y));
            }

            Hitbox = new ConvexPolygonHitbox(vertices);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public void DrawGeometry(Camera2D camera)
        {
            // Draw pentagon using the hitbox vertices
            var polygonHitbox = (PolygonHitbox)Hitbox;

            // Transform vertices to world space
            var worldVertices = GeometryHelper.TransformToWorldSpace(polygonHitbox.Vertices, Position, Rotation);

            // Draw filled pentagon
            Raylib.DrawPoly(Position, polygonHitbox.Vertices.Count, 1.5f, Rotation, Color.Red);

            // Draw outline by connecting vertices
            for (int i = 0; i < worldVertices.Count; i++)
            {
                int nextIndex = (i + 1) % worldVertices.Count;
                Raylib.DrawLineV(worldVertices[i], worldVertices[nextIndex], Color.Black);
            }
        }
    }
}
