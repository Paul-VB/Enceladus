using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using System.Collections.Concurrent;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionChecker
    {
        List<CollisionResult> CheckEntitiesToCells(IEnumerable<MovableEntity> entities, Map map);
        List<CollisionResult> CheckEntitiesToEntities(IEntityRegistry entityRegistry);
    }

    public class CollisionChecker : ICollisionChecker
    {
        private readonly IAabbCollisionDetector _aabbCollisionDetector;
        private readonly ISatCollisionDetector _satCollisionDetector;

        public CollisionChecker(IAabbCollisionDetector aabbCollisionDetector, ISatCollisionDetector satCollisionDetector)
        {
            _aabbCollisionDetector = aabbCollisionDetector;
            _satCollisionDetector = satCollisionDetector;
        }

        public List<CollisionResult> CheckEntitiesToCells(IEnumerable<MovableEntity> entities, Map map)
        {
            var collisions = new ConcurrentBag<CollisionResult>();
            //todo: stretch goal, can we leverage GPU for this?
            Parallel.ForEach(entities, entity =>
            {
                var entityCollisions = CheckEntityToCells(entity, map);
                foreach (var collision in entityCollisions)
                {
                    collisions.Add(collision);
                }
            });

            return collisions.ToList();
        }

        private List<CollisionResult> CheckEntityToCells(MovableEntity entity, Map map)
        {
            var collisions = new List<CollisionResult>();

            // Broad check (AABB)
            var cellCollisionCandiates = _aabbCollisionDetector.CheckPotentialCellCollisions(entity, map);
            if (cellCollisionCandiates.Count == 0) return collisions;

            //temp placeholder
            //todo: actually implement narrow circle to anything collision
            var placeHolderCircleAlgo = (MovableEntity e, ICollidable c) =>
            {
                return new CollisionResult
                {
                    Entity = e,
                    OtherObject = c,
                    PenetrationDepth = 0,
                    CollisionNormal = System.Numerics.Vector2.Zero
                };
            };

            // Narrow check
            Func<MovableEntity, ICollidable, CollisionResult> narrowCollisionAlgorithm =
                entity.Hitbox is RectHitbox || entity.Hitbox is PolygonHitbox ? _satCollisionDetector.CheckCollision : placeHolderCircleAlgo;

            foreach (var cell in cellCollisionCandiates)
            {
                var collision = narrowCollisionAlgorithm(entity, cell);
                if (collision.PenetrationDepth > 0)
                {
                    collisions.Add(collision);
                }
            }

            return collisions;
        }

        public List<CollisionResult> CheckEntitiesToEntities(IEntityRegistry entityRegistry)
        {
            var collisions = new List<CollisionResult>();
            var moveables = entityRegistry.MovableEntities;
            var statics = entityRegistry.StaticEntities;

            // Moveable vs Moveable
            for (int i = 0; i < moveables.Count; i++)
            {
                for (int j = i + 1; j < moveables.Count; j++)
                {
                    CheckPair(moveables[i], moveables[j], collisions);
                }
            }

            // Moveable vs Static
            foreach (var moveable in moveables)
            {
                foreach (var staticEntity in statics)
                {
                    CheckPair(moveable, staticEntity, collisions);
                }
            }

            return collisions;
        }

        private void CheckPair(MovableEntity moveable, Entity other, List<CollisionResult> collisions)
        {
            // Broad phase: AABB check
            if (!_aabbCollisionDetector.CheckPotentialCollision(moveable, other))
                return;

            //todo: also implement circle to anything for e to e collisions

            // Narrow phase: For now, only check polygon-to-polygon collisions
            if ((moveable.Hitbox is RectHitbox || moveable.Hitbox is PolygonHitbox) &&
                (other.Hitbox is RectHitbox || other.Hitbox is PolygonHitbox))
            {
                var result = _satCollisionDetector.CheckCollision(moveable, other);
                if (result.PenetrationDepth > 0)
                {
                    collisions.Add(result);
                }
            }
        }
    }
}
