using Enceladus.Core.Physics.Hitboxes;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Entities.TestMonsters
{
    public class MenacingRedPentagon : MoveableEntity, ICollidableEntity
    {
        public Hitbox Hitbox { get; set; }

        public MenacingRedPentagon()
        {
            // 3x3 unit pentagon - regular pentagon centered at origin
            var vertices = new List<Vector2>();
            float radius = 1.5f;

            for (int i = 0; i < 5; i++)
            {
                float angle = (i * 72f - 90f) * (MathF.PI / 180f); // Start from top
                float x = radius * MathF.Cos(angle);
                float y = radius * MathF.Sin(angle);
                vertices.Add(new Vector2(x, y));
            }

            Hitbox = new PolygonHitbox(vertices);
            Mass = 500f;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(Camera2D camera)
        {
            // Draw pentagon using the hitbox vertices
            var polygonHitbox = (PolygonHitbox)Hitbox;

            // Rotate and translate vertices to world space
            float radians = Rotation * (MathF.PI / 180f);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            var worldVertices = new Vector2[polygonHitbox.Vertices.Count];
            for (int i = 0; i < polygonHitbox.Vertices.Count; i++)
            {
                var vertex = polygonHitbox.Vertices[i];
                float rotatedX = vertex.X * cos - vertex.Y * sin;
                float rotatedY = vertex.X * sin + vertex.Y * cos;
                worldVertices[i] = new Vector2(rotatedX + Position.X, rotatedY + Position.Y);
            }

            // Draw filled pentagon
            Raylib.DrawPoly(Position, polygonHitbox.Vertices.Count, 1.5f, Rotation, Color.Red);

            // Draw outline by connecting vertices
            for (int i = 0; i < worldVertices.Length; i++)
            {
                int nextIndex = (i + 1) % worldVertices.Length;
                Raylib.DrawLineV(worldVertices[i], worldVertices[nextIndex], Color.Black);
            }
        }
    }
}
