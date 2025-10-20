using Enceladus.Core.Config;
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Entities.TestMonsters
{
    public class AwfulGreenStar : MovableEntity, ICollidable
    {
        private readonly IInputManager _inputManager;
        public override IHitbox Hitbox { get; set; }

        private readonly float _thrust = 5000f;

        public AwfulGreenStar(IInputManager inputManager, IConfigService configService)
            : base(configService)
        {
            _inputManager = inputManager;
            Mass = 50f;
        }

        public override void Update(float deltaTime)
        {
            HandleInput(deltaTime);
            base.Update(deltaTime);
        }

        private void HandleInput(float deltaTime)
        {
            var movementInput = _inputManager.GetArrowKeyMovementInput();
            if (movementInput != Vector2.Zero)
            {
                Accelerate(movementInput * _thrust, deltaTime);
            }
        }

        public override void Draw(Camera2D camera)
        {
            var concaveHitbox = (ConcavePolygonHitbox)Hitbox;

            // Transform outer vertices to world space for outline
            var worldVertices = GeometryHelper.TransformToWorldSpace(concaveHitbox.Vertices, Position, Rotation);

            // Draw each convex slice as filled triangles
            foreach (var slice in concaveHitbox.ConvexSlices)
            {
                var sliceWorldVertices = GeometryHelper.TransformToWorldSpace(slice.Vertices, Position, Rotation);

                // Each slice from ear clipping is already a triangle (3 vertices)
                if (sliceWorldVertices.Count == 3)
                {
                    // Try both winding orders - DrawTriangle may require counter-clockwise
                    Raylib.DrawTriangle(sliceWorldVertices[2], sliceWorldVertices[1], sliceWorldVertices[0], Color.Green);
                }
                else
                {
                    // Fallback: fan triangulation from first vertex for non-triangle slices
                    for (int i = 1; i < sliceWorldVertices.Count - 1; i++)
                    {
                        Raylib.DrawTriangle(sliceWorldVertices[0], sliceWorldVertices[i], sliceWorldVertices[i + 1], Color.Green);
                    }
                }
            }

            // Draw outline
            for (int i = 0; i < worldVertices.Count; i++)
            {
                int nextIndex = (i + 1) % worldVertices.Count;
                Raylib.DrawLineV(worldVertices[i], worldVertices[nextIndex], Color.DarkGreen);
            }
        }
    }
}
