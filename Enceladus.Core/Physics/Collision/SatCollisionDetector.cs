using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{

    public interface ISatCollisionDetector
    {
        CollisionResult CheckCollision(MovableEntity entity, ICollidable otherObject);
    }

    public class SatCollisionDetector : ISatCollisionDetector
    {
        private readonly IAxesExtractor _axesExtractor;

        public SatCollisionDetector(IAxesExtractor axesExtractor)
        {
            _axesExtractor = axesExtractor;
        }

        public CollisionResult CheckCollision(MovableEntity entity, ICollidable otherObject)
        {
            var collisionInfo = (entity.Hitbox, otherObject.Hitbox) switch
            {
                (ConcavePolygonHitbox e, CellHitbox c) => CheckConcaveToCell(e, entity, c, otherObject),
                (ConvexPolygonHitbox e, CellHitbox c) => CheckConvexToCell(e, entity, c, otherObject),
                (CircleHitbox e, CellHitbox c) => CheckCircleToCell(e, entity, c, otherObject),

                (ConcavePolygonHitbox e, ConcavePolygonHitbox o) => CheckConcaveToConcave(e, entity, o, otherObject),
                (ConcavePolygonHitbox e, ConvexPolygonHitbox o) => CheckConcaveToConvex(e, entity, o, otherObject),
                (ConcavePolygonHitbox e, CircleHitbox o) => CheckConcaveToCircle(e, entity, o, otherObject),

                (ConvexPolygonHitbox e, ConcavePolygonHitbox o) => CheckConvexToConcave(e, entity, o, otherObject),
                (ConvexPolygonHitbox e, ConvexPolygonHitbox o) => CheckConvexToConvex(e, entity, o, otherObject),
                (ConvexPolygonHitbox e, CircleHitbox o) => CheckConvexToCircle(e, entity, o, otherObject),

                (CircleHitbox e, ConcavePolygonHitbox o) => CheckCircleToConcave(e, entity, o, otherObject),
                (CircleHitbox e, ConvexPolygonHitbox o) => CheckCircleToConvex(e, entity, o, otherObject),
                (CircleHitbox e, CircleHitbox o) => throw new Exception("SAT collision logic should not be used for circle to circle collision detection. use the simple radius math only for this"),

                _ => throw new NotSupportedException($"Collision between {entity.Hitbox?.GetType()} and {otherObject.Hitbox?.GetType()} is not supported")
            };

            var collisionResult = new CollisionResult()
            {
                Entity = entity,
                OtherObject = otherObject,
                PenetrationDepth = collisionInfo.PenetrationDepth,
                CollisionNormal = collisionInfo.CollisionNormal
            };

            return collisionResult;
        }

                #region Cell Collision (Optimized)

        // Static axes for all cells (axis-aligned rectangle - these never change!)
        private static readonly List<Vector2> CELL_AXES = new() { new(1, 0), new(0, 1) };

        private CollisionInfo CheckConvexToCell(ConvexPolygonHitbox h1, MovableEntity c1, CellHitbox h2, ICollidable c2)
        {
            var h1Vertices = GeometryHelper.TransformToWorldSpace(h1.Vertices, c1.Position, c1.Rotation);
            var h2Vertices = h2.PretransformedVertices;

            var h1Axes = _axesExtractor.ExtractAxes(h1Vertices, h1);
            var h2Axes = CELL_AXES;

            var collisionInfo = CheckSatCollision(h1Vertices, h2Vertices, h1Axes.Concat(h2Axes).ToList());
            return collisionInfo;
        }

        private CollisionInfo CheckConcaveToCell(ConcavePolygonHitbox h1, MovableEntity c1, CellHitbox h2, ICollidable c2)
        {
            var h2Vertices = h2.PretransformedVertices;
            var h2Axes = CELL_AXES;

            var collisionInfos = new List<CollisionInfo>();

            foreach (var slice in h1.ConvexSlices)
            {
                var sliceVertices = GeometryHelper.TransformToWorldSpace(slice.Vertices, c1.Position, c1.Rotation);
                var sliceAxes = _axesExtractor.ExtractAxes(sliceVertices, slice);

                var collisionInfo = CheckSatCollision(sliceVertices, h2Vertices, sliceAxes.Concat(h2Axes).ToList());
                collisionInfos.Add(collisionInfo);
            }

            return GetDeepestCollision(collisionInfos);
        }

        private CollisionInfo CheckCircleToCell(CircleHitbox h1, MovableEntity c1, CellHitbox h2, ICollidable c2)
        {
            throw new NotImplementedException("CircleToCell collision not yet implemented");
        }

        #endregion

        #region concave to other
        private CollisionInfo CheckConcaveToConcave(ConcavePolygonHitbox h1, ICollidable c1, ConcavePolygonHitbox h2, ICollidable c2)
        {
            var collisionInfos = new List<CollisionInfo>();

            // Double loop through all slice pairs
            foreach (var slice1 in h1.ConvexSlices)
            {
                var slice1Vertices = GeometryHelper.TransformToWorldSpace(slice1.Vertices, c1.Position, c1.Rotation);
                var slice1Axes = _axesExtractor.ExtractAxes(slice1Vertices, slice1);

                foreach (var slice2 in h2.ConvexSlices)
                {
                    var slice2Vertices = GeometryHelper.TransformToWorldSpace(slice2.Vertices, c2.Position, c2.Rotation);
                    var slice2Axes = _axesExtractor.ExtractAxes(slice2Vertices, slice2);

                    var collisionInfo = CheckSatCollision(slice1Vertices, slice2Vertices, slice1Axes.Concat(slice2Axes).ToList());
                    collisionInfos.Add(collisionInfo);
                }
            }

            // Return the deepest collision from all slice pair checks
            return GetDeepestCollision(collisionInfos);
        }

        private CollisionInfo CheckConcaveToConvex(ConcavePolygonHitbox h1, ICollidable c1, ConvexPolygonHitbox h2, ICollidable c2)
        {
            // Transform convex hitbox vertices once (outside loop)
            var h2Vertices = GeometryHelper.TransformToWorldSpace(h2.Vertices, c2.Position, c2.Rotation);
            var h2Axes = _axesExtractor.ExtractAxes(h2Vertices, h2);

            var collisionInfos = new List<CollisionInfo>();

            // Loop through each slice of the concave polygon
            foreach (var slice in h1.ConvexSlices)
            {
                var sliceVertices = GeometryHelper.TransformToWorldSpace(slice.Vertices, c1.Position, c1.Rotation);
                var sliceAxes = _axesExtractor.ExtractAxes(sliceVertices, slice);

                var collisionInfo = CheckSatCollision(sliceVertices, h2Vertices, sliceAxes.Concat(h2Axes).ToList());
                collisionInfos.Add(collisionInfo);
            }

            // Return the deepest collision from all slice checks
            return GetDeepestCollision(collisionInfos);
        }

        private CollisionInfo CheckConcaveToCircle(ConcavePolygonHitbox h1, ICollidable c1, CircleHitbox h2, ICollidable c2)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region convex to other
        private CollisionInfo CheckConvexToConcave(ConvexPolygonHitbox h1, ICollidable c1, ConcavePolygonHitbox h2, ICollidable c2)
        {
            return ReverseCollisionNormal(CheckConcaveToConvex(h2, c2, h1, c1));
        }

        private CollisionInfo CheckConvexToConvex(ConvexPolygonHitbox h1, ICollidable c1, ConvexPolygonHitbox h2, ICollidable c2)
        {
            var h1Vertices = GeometryHelper.TransformToWorldSpace(h1.Vertices, c1.Position, c1.Rotation);
            var h2Vertices = GeometryHelper.TransformToWorldSpace(h2.Vertices, c2.Position, c2.Rotation);

            var h1Axes = _axesExtractor.ExtractAxes(h1Vertices, h1);
            var h2Axes = _axesExtractor.ExtractAxes(h2Vertices, h2);

            var collisionInfo = CheckSatCollision(h1Vertices, h2Vertices, h1Axes.Concat(h2Axes).ToList());
            return collisionInfo;
        }

        private CollisionInfo CheckConvexToCircle(ConvexPolygonHitbox h1, ICollidable c1, CircleHitbox h2, ICollidable c2)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region circle to other
        private CollisionInfo CheckCircleToConcave(CircleHitbox h1, ICollidable c1, ConcavePolygonHitbox h2, ICollidable c2)
        {
            return ReverseCollisionNormal(CheckConcaveToCircle(h2, c2, h1, c1));
        }

        private CollisionInfo CheckCircleToConvex(CircleHitbox h1, ICollidable c1, ConvexPolygonHitbox h2, ICollidable c2)
        {
            return ReverseCollisionNormal(CheckConvexToCircle(h2, c2, h1, c1));
        }
        #endregion


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

        private CollisionInfo ReverseCollisionNormal(CollisionInfo collisionInfo)
        {
            collisionInfo.CollisionNormal *= -1;
            return collisionInfo;
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
