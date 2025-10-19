using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface IVertexTransformer
    {
        List<Vector2> TransformToWorldSpace(List<Vector2> localVertices, Vector2 position, float rotation);
    }

    public class VertexTransformer : IVertexTransformer
    {
        public List<Vector2> TransformToWorldSpace(List<Vector2> localVertices, Vector2 position, float rotation)
        {
            float radians = AngleHelper.DegToRad(rotation);
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);

            var worldVertices = new List<Vector2>();

            foreach (var vertex in localVertices)
            {
                // Rotate vertex
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
