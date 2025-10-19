using Enceladus.Core.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{

    public interface ISatCollisionDetector
    {
        CollisionResult CheckCollision(MovableEntity entity, ICollidable otherObject);
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

        //public CollisionResult CheckCollision(MovableEntity entity1, ICollidable otherObject)
        //{
        //    var vertices1 = _vertexExtractor.ExtractWorldVertices(entity1);
        //    var vertices2 = _vertexExtractor.ExtractWorldVertices(otherObject);

        //    var axes1 = _axesExtractor.ExtractAxes(vertices1, entity1.Hitbox);
        //    var axes2 = _axesExtractor.ExtractAxes(vertices2, otherObject.Hitbox);

        //    var collisionInfo = CheckSatCollision(vertices1, vertices2, axes1.Concat(axes2).ToList());

        //    var collisionResult = new CollisionResult()
        //    {
        //        Entity = entity1,
        //        OtherObject = otherObject,
        //        PenetrationDepth = collisionInfo.PenetrationDepth,
        //        CollisionNormal = collisionInfo.CollisionNormal
        //    };
        //    return collisionResult;
        //}

        public CollisionResult CheckCollision(MovableEntity entity, ICollidable otherObject)
        {
            var entityWorldVerticeses = _vertexExtractor.ExtractWorldVerticeses(entity);
            var otherObjectWorldVerticeses = _vertexExtractor.ExtractWorldVerticeses(otherObject);

            var collisionInfos = new List<CollisionInfo>();

            foreach(var entityVertecies in entityWorldVerticeses)
            {
                foreach(var otherObjectVertecies in otherObjectWorldVerticeses)
                {
                    var entityAxes = _axesExtractor.ExtractAxes(entityVertecies, entity.Hitbox);
                    var otherObjectAxes = _axesExtractor.ExtractAxes(otherObjectVertecies, otherObject.Hitbox);

                    var collisionInfo = CheckSatCollision(entityVertecies, otherObjectVertecies, entityAxes.Concat(otherObjectAxes).ToList());
                    collisionInfos.Add(collisionInfo);
                }
            }

            var deepestCollisioninfo = GetDeepestCollision(collisionInfos);

            var collisionResult = new CollisionResult()
            {
                Entity = entity,
                OtherObject = otherObject,
                PenetrationDepth = deepestCollisioninfo.PenetrationDepth,
                CollisionNormal = deepestCollisioninfo.CollisionNormal
            };

            return collisionResult;

        }

        private CollisionInfo GetDeepestCollision(List<CollisionInfo> collisionInfos)
        {
            var deepestCollisionInfo = new CollisionInfo()
            {
                PenetrationDepth = 0f,
                CollisionNormal = Vector2.Zero
            };

            foreach (var collisionInfo in collisionInfos)
            {
                if (collisionInfo.PenetrationDepth > deepestCollisionInfo.PenetrationDepth)
                    deepestCollisionInfo = collisionInfo;
            }

            return deepestCollisionInfo;
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
                    //found at least one axis where we can find a gap. the shapes cannot be colliding
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

            //todo: explain why we need to make sure the normal is from cell to entity? this function gets used by entity to entity collisions so logic that pertains to cells dosnt go here
            //todo: also explain why we need the center of vertecies?
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
