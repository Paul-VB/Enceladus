using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{

    public interface ISatCollisionDetector
    {
        EntityToEntityCollisionResult CheckCollision(ICollidableEntity entity1, ICollidableEntity entity2);
        EntityToCellCollisionResult CheckCollision(ICollidableEntity entity, Cell cell);
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

        public EntityToEntityCollisionResult CheckCollision(ICollidableEntity entity1, ICollidableEntity entity2)
        {
            var vertices1 = _vertexExtractor.ExtractWorldVertices(entity1);
            var vertices2 = _vertexExtractor.ExtractWorldVertices(entity2);

            var axes1 = _axesExtractor.ExtractAxes(vertices1, entity1.Hitbox);
            var axes2 = _axesExtractor.ExtractAxes(vertices2, entity2.Hitbox);

            var collisionInfo = CheckSatCollision(vertices1, vertices2, axes1.Concat(axes2).ToList());

            var collisionResult = new EntityToEntityCollisionResult()
            {
                Entity = entity1,
                OtherEntity = entity2,
                PenetrationDepth = collisionInfo.PenetrationDepth,
                CollisionNormal = collisionInfo.CollisionNormal
            };
            return collisionResult;
        }

        public EntityToCellCollisionResult CheckCollision(ICollidableEntity entity, Cell cell)
        {
            var entityVertices = _vertexExtractor.ExtractWorldVertices(entity);
            var cellVertices = _vertexExtractor.ExtractWorldVertices(cell);

            var entityAxes = _axesExtractor.ExtractAxes(entityVertices, entity.Hitbox);
            var cellAxes = _axesExtractor.ExtractAxes(cellVertices, new RectHitbox(1, 1)); // Cell is always 1x1 rect

            var collisionInfo = CheckSatCollision(entityVertices, cellVertices, entityAxes.Concat(cellAxes).ToList());

            var collisionResult = new EntityToCellCollisionResult()
            {
                Entity = entity,
                Cell = cell,
                PenetrationDepth = collisionInfo.PenetrationDepth,
                CollisionNormal = collisionInfo.CollisionNormal
            };
            return collisionResult;
        }

        private CollisionInfo CheckSatCollision(List<Vector2> vertices1, List<Vector2> vertices2, List<Vector2> axes)
        {
            var minPenetration = float.MaxValue;
            var minAxis = Vector2.Zero;

            foreach (var axis in axes)
            {
                var penetration = GetAxisPenetration(axis, vertices1, vertices2);

                if (penetration == 0)
                {
                    return new CollisionInfo
                    {
                        PenetrationDepth = 0,
                        CollisionNormal = new Vector2()
                    };
                }

                // Track the axis with minimum penetration
                if (penetration < minPenetration)
                {
                    minPenetration = penetration;
                    minAxis = axis;
                }
            }

            // All axes overlap - collision detected
            // Ensure normal points from shape2 to shape1 (from cell to entity)
            var centerDiff = GetCenterOfVertices(vertices1) - GetCenterOfVertices(vertices2);
            if (Vector2.Dot(minAxis, centerDiff) < 0)
            {
                minAxis = -minAxis;
            }

            return new CollisionInfo
            {
                PenetrationDepth = minPenetration,
                CollisionNormal = minAxis
            };
        }

        private float GetAxisPenetration(Vector2 axis, List<Vector2> vertices1, List<Vector2> vertices2)
        {
            var (min1, max1) = ProjectShapeOntoAxis(axis, vertices1);
            var (min2, max2) = ProjectShapeOntoAxis(axis, vertices2);

            // Check if projections overlap
            bool overlaps = min1 <= max2 && min2 <= max1;
            if (!overlaps)
            {
                return 0f;
            }

            // Calculate how deeply the projections penetrate
            float penetration = Math.Min(max1 - min2, max2 - min1);
            return penetration;
        }

        private Vector2 GetCenterOfVertices(List<Vector2> vertices)
        {
            Vector2 sum = Vector2.Zero;
            foreach (var vertex in vertices)
            {
                sum += vertex;
            }
            return sum / vertices.Count;
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

        private class CollisionInfo
        {
            public float PenetrationDepth { get; set; }
            public Vector2 CollisionNormal { get; set; }
        }
    }

}
