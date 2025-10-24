using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision.Detection
{
    public interface ICircleCollisionDetector
    {
        CollisionResult CheckCollision(MovableEntity entity, ICollidable otherObject);

    }
    public class CircleCollisionDetector : ICircleCollisionDetector
    {
        private readonly ICollisionInfoService _collisionInfoService;
        public CircleCollisionDetector(ICollisionInfoService collisionInfoService)
        {
            _collisionInfoService = collisionInfoService;
        }
        public CollisionResult CheckCollision(MovableEntity entity, ICollidable otherObject)
        {
            var collisionInfo = (entity.Hitbox, otherObject.Hitbox) switch
            {
                (CircleHitbox e, CellHitbox c) => CheckCircleToCell(e, entity, c, otherObject),
                (CircleHitbox e, ConcavePolygonHitbox o) => CheckCircleToConcave(e, entity, o, otherObject),
                (CircleHitbox e, ConvexPolygonHitbox o) => CheckCircleToConvex(e, entity, o, otherObject),
                (CircleHitbox e, CircleHitbox o) => CheckCircleToCircle(e, entity, o, otherObject),

                // Reversed cases: polygon entity colliding with circle other
                (ConcavePolygonHitbox e, CircleHitbox o) => CheckConcaveToCircle(e, entity, o, otherObject),
                (ConvexPolygonHitbox e, CircleHitbox o) => CheckConvexToCircle(e, entity, o, otherObject),

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

        private CollisionInfo CheckCircleToCell(CircleHitbox h1, MovableEntity c1, CellHitbox h2, ICollidable c2)
        {
            var topLeftCorner = h2.PretransformedVertices[0];
            var bottomRightCorner = h2.PretransformedVertices[2];

            //dont worry if this line looks weird. i promise its a shortcut that AABB rectangls like cells can take to get the closet point on their permiter. trust me bro
            var closestPoint = Vector2.Clamp(c1.Position, topLeftCorner, bottomRightCorner);
            var distanceVector = c1.Position - closestPoint;
            var distanceSquared = distanceVector.LengthSquared();

            // Check if circle overlaps the rectangle
            var radiusSquared = h1.Radius * h1.Radius;

            //we compare the squared distances because doing sqrt is a bit more expensive. this just saves cpu cycles
            if (distanceSquared >= radiusSquared)
            {
                return CollisionInfo.NonCollision;
            }

            var distance = MathF.Sqrt(distanceSquared); 
            var penetration = h1.Radius - distance;
            var normal = GeometryHelper.NormalizeByDistance(distanceVector, distance); 

            return new CollisionInfo
            {
                PenetrationDepth = penetration,
                CollisionNormal = normal
            };
        }

        private CollisionInfo CheckCircleToConcave(CircleHitbox h1, ICollidable c1, ConcavePolygonHitbox h2, ICollidable c2)
        {
            var collisionInfos = new List<CollisionInfo>();

            foreach (var slice in h2.ConvexSlices)
            {
                var collisionInfo = ComputeCircleToConvexVertices(h1, c1, slice.Vertices, c2);
                if (collisionInfo.PenetrationDepth > 0)
                    collisionInfos.Add(collisionInfo);
            }

            return _collisionInfoService.GetDeepestCollision(collisionInfos);
        }

        private CollisionInfo CheckCircleToConvex(CircleHitbox h1, ICollidable c1, ConvexPolygonHitbox h2, ICollidable c2)
        {
            return ComputeCircleToConvexVertices(h1, c1, h2.Vertices, c2);
        }

        private CollisionInfo ComputeCircleToConvexVertices(CircleHitbox h1, ICollidable c1, List<Vector2> ConvexVertices, ICollidable c2)
        {
            var h2Vertices = GeometryHelper.TransformToWorldSpace(ConvexVertices, c2.Position, c2.Rotation);
            (var closestPoint, var distanceSquared) = GetClosestPointOnPolygon(c1.Position, h2Vertices);

            var radiusSquared = h1.Radius * h1.Radius;
            if (distanceSquared > radiusSquared)
            {
                return CollisionInfo.NonCollision;
            }

            var distance = MathF.Sqrt(distanceSquared);
            var distanceVector = c1.Position - closestPoint;

            return new CollisionInfo
            {
                PenetrationDepth = h1.Radius - distance,
                CollisionNormal = distanceVector / distance
            };
        }

        private CollisionInfo CheckCircleToCircle(CircleHitbox h1, MovableEntity c1, CircleHitbox h2, ICollidable c2)
        {
            var minDistance = h1.Radius + h2.Radius;
            //we compare the squared distances because doing sqrt is a bit more expensive. this just saves cpu cycles
            var actualDistanceSquared = Vector2.DistanceSquared(c1.Position, c2.Position);
            var minDistanceSquared = minDistance * minDistance;

            if (minDistanceSquared < actualDistanceSquared) 
                return CollisionInfo.NonCollision;

            var actualDistance = (float)Math.Sqrt(actualDistanceSquared);
            var penetrationDepth = minDistance - actualDistance;
            var collisionVector = c1.Position - c2.Position; 

            //dividing by the already computed actual distance is cheaper than using Vector2.Normalize()
            var normalizedCollisionVector = collisionVector / actualDistance; 

            return new CollisionInfo()
            {
                PenetrationDepth = penetrationDepth,
                CollisionNormal = normalizedCollisionVector
            };
        }

        private Vector2 GetClosestPointOnLine(Vector2 circleCenter, Vector2 vertexA, Vector2 vertexB)
        {
            Vector2 ab = vertexB - vertexA;           // Edge vector (arrow from A to B)
            Vector2 ap = circleCenter - vertexA;           // Vector from A to point P

            // How far along AB is the closest point? (as a fraction 0 to 1)
            float t = Vector2.Dot(ap, ab) / Vector2.Dot(ab, ab);

            // Clamp t to [0, 1] to stay on the segment
            t = Math.Clamp(t, 0f, 1f);

            // Calculate the actual closest point
            return vertexA + t * ab;
        }

        private (Vector2 closestPoint, float distanceSquared) GetClosestPointOnPolygon(Vector2 circleCenter, List<Vector2> polygonVertecies)
        {
            var closestPoint = Vector2.Zero;
            var distanceSquared = float.MaxValue; //we save square-rooting until the end because its expesnive

            for (int a = 0; a < polygonVertecies.Count; a++)
            {
                var b = a + 1 != polygonVertecies.Count ? a + 1 : 0;
                var vertexA = polygonVertecies[a];
                var vertexB = polygonVertecies[b];
                var pointOnLine = GetClosestPointOnLine(circleCenter, vertexA, vertexB);
                var pointOnLineDistanceSquared = Vector2.DistanceSquared(pointOnLine, circleCenter);

                if (pointOnLineDistanceSquared <= distanceSquared)
                {
                    closestPoint = pointOnLine;
                    distanceSquared = pointOnLineDistanceSquared;
                }
            }

            return (closestPoint, distanceSquared);
        }

        private CollisionInfo CheckConvexToCircle(ConvexPolygonHitbox h1, ICollidable c1, CircleHitbox h2, ICollidable c2)
        {
            return _collisionInfoService.ReverseCollisionNormal(CheckCircleToConvex(h2, c2, h1, c1));
        }

        private CollisionInfo CheckConcaveToCircle(ConcavePolygonHitbox h1, ICollidable c1, CircleHitbox h2, ICollidable c2)
        {
            return _collisionInfoService.ReverseCollisionNormal(CheckCircleToConcave(h2, c2, h1, c1));
        }

    }
}
