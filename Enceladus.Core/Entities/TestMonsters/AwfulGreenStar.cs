using Enceladus.Core.Input;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Physics.Motion.MotionControllers;
using Enceladus.Core.Rendering;
using Enceladus.Core.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Entities.TestMonsters
{
    public class AwfulGreenStar : MovableEntity, IGeometryRendered
    {
        public override IHitbox Hitbox { get; set; }
        public override MotionControllerType MotionControllerType { get; set; } = MotionControllerType.ArrowKeys;

        public void DrawGeometry(Camera2D camera)
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
