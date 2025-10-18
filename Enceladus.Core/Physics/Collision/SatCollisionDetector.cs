using Enceladus.Core.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{

    public interface ISatCollisionDetector
    {
        BaseCollisionResult CheckCollision(MoveableEntity entity, ICollidable otherObject);
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

        public BaseCollisionResult CheckCollision(MoveableEntity entity1, ICollidable otherObject)
        {
            var vertices1 = _vertexExtractor.ExtractWorldVertices(entity1);
            var vertices2 = _vertexExtractor.ExtractWorldVertices(otherObject);

            var axes1 = _axesExtractor.ExtractAxes(vertices1, entity1.Hitbox);
            var axes2 = _axesExtractor.ExtractAxes(vertices2, otherObject.Hitbox);

            var collisionInfo = CheckSatCollision(vertices1, vertices2, axes1.Concat(axes2).ToList());

            var collisionResult = new BaseCollisionResult()
            {
                Entity = entity1,
                OtherObject = otherObject,
                PenetrationDepth = collisionInfo.PenetrationDepth,
                CollisionNormal = collisionInfo.CollisionNormal
            };
            return collisionResult;
        }

        //todo: we need some way of supporting concave polygons too. this would take care of one of the critical things on the main todo from claudeCode. 
        //whats an effiicent way to do this? split up the polygon into the smallest number of convex polygons? maybe we could add a function somewhere to take in a polygon, and if its already convex return it. if its concave, do math and eventually return a list of convex polygons.
        //but also wait... this seems like expensive math to do each frame. i think if we have a hitbox that is concave we can compute the convex component hotboxes once and save it to the entity. maybe the polygon hitbox should be a list<ConcavePolygon> and we make a new class ConcavePolygon : List<Vector2> 
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
