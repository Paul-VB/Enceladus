using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ISatCollisionDetector
    {
        bool CheckCollision(Entity entity1, Entity entity2);
        bool CheckCollision(Entity entity, Cell cell);
    }

    public class SatCollisionDetector : ISatCollisionDetector
    {
        private readonly IVertexExtractor _vertexExtractor;
        private readonly IAxesExtractor _axesExtractor;

        public SatCollisionDetector(IVertexExtractor vertexExtractor, IAxesExtractor axesExtractor)
        {
            _vertexExtractor = vertexExtractor;
            _axesExtractor = axesExtractor;
        }

        public bool CheckCollision(Entity entity1, Entity entity2)
        {
            var vertices1 = _vertexExtractor.ExtractWorldVertices(entity1);
            var vertices2 = _vertexExtractor.ExtractWorldVertices(entity2);

            var axes1 = _axesExtractor.ExtractAxes(vertices1, entity1.Hitbox);
            var axes2 = _axesExtractor.ExtractAxes(vertices2, entity2.Hitbox);

            return CheckSatCollision(vertices1, vertices2, axes1.Concat(axes2).ToList());
        }

        public bool CheckCollision(Entity entity, Cell cell)
        {
            var entityVertices = _vertexExtractor.ExtractWorldVertices(entity);
            var cellVertices = _vertexExtractor.ExtractWorldVertices(cell);

            var entityAxes = _axesExtractor.ExtractAxes(entityVertices, entity.Hitbox);
            var cellAxes = _axesExtractor.ExtractAxes(cellVertices, new RectHitbox(1, 1)); // Cell is always 1x1 rect

            return CheckSatCollision(entityVertices, cellVertices, entityAxes.Concat(cellAxes).ToList());
        }

        private bool CheckSatCollision(List<Vector2> vertices1, List<Vector2> vertices2, List<Vector2> axes)
        {
            foreach (var axis in axes)
                if (!ProjectionsOverlap(axis, vertices1, vertices2))
                   return false;

            return true;
        }

        private bool ProjectionsOverlap(Vector2 axis, List<Vector2> vertices1, List<Vector2> vertices2)
        {
            // Project shape 1 onto the axis
            var (min1, max1) = ProjectShapeOntoAxis(axis, vertices1);

            // Project shape 2 onto the axis
            var (min2, max2) = ProjectShapeOntoAxis(axis, vertices2);

            // Check if the projections overlap
            // They overlap if: min1 <= max2 AND min2 <= max1
            return min1 <= max2 && min2 <= max1;
        }

        private (float min, float max) ProjectShapeOntoAxis(Vector2 axis, List<Vector2> vertices)
        {
            // Project the first vertex to initialize min and max
            float min = Vector2.Dot(vertices[0], axis);
            float max = min;

            // Project remaining vertices and track min/max
            for (int i = 1; i < vertices.Count; i++)
            {
                float projection = Vector2.Dot(vertices[i], axis);
                min = Math.Min(min, projection);
                max = Math.Max(max, projection);
            }

            return (min, max);
        }
    }
}
