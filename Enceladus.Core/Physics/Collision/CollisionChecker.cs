using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Collision.Detection;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using System.Collections.Concurrent;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionChecker
    {
        List<CollisionResult> CheckEntitiesToCells();
        List<CollisionResult> CheckEntitiesToEntities();
    }

    public class CollisionChecker : ICollisionChecker
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly IWorldService _worldService;
        private readonly IAabbCollisionDetector _aabbCollisionDetector;
        private readonly ISatCollisionDetector _satCollisionDetector;
        private readonly ICircleCollisionDetector _circleCollisionDetector;

        public CollisionChecker(IEntityRegistry entityRegistry, IWorldService worldService, IAabbCollisionDetector aabbCollisionDetector, ISatCollisionDetector satCollisionDetector, ICircleCollisionDetector circleCollisionDetector)
        {
            _entityRegistry = entityRegistry;
            _worldService = worldService;
            _aabbCollisionDetector = aabbCollisionDetector;
            _satCollisionDetector = satCollisionDetector;
            _circleCollisionDetector = circleCollisionDetector;
        }

        public List<CollisionResult> CheckEntitiesToCells()
        {
            var collisions = new ConcurrentBag<CollisionResult>();
            //todo: stretch goal, can we leverage GPU for this?
            Parallel.ForEach(_entityRegistry.MovableEntities, entity =>
            {
                var entityCollisions = CheckEntityToCells(entity, _worldService.CurrentMap);
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

            // Narrow check - dispatch to appropriate detector based on hitbox type
            Func<MovableEntity, ICollidable, CollisionResult> narrowCollisionAlgorithm;
            if (entity.Hitbox is CircleHitbox)
                narrowCollisionAlgorithm = _circleCollisionDetector.CheckCollision;
            else
                narrowCollisionAlgorithm = _satCollisionDetector.CheckCollision;

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

        public List<CollisionResult> CheckEntitiesToEntities()
        {
            var collisions = new List<CollisionResult>();
            var moveables = _entityRegistry.MovableEntities;

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
                foreach (var staticEntity in _entityRegistry.StaticEntities)
                {
                    CheckPair(moveable, staticEntity, collisions);
                }
            }

            return collisions;
        }

        private void CheckPair(MovableEntity moveable, Entity other, List<CollisionResult> collisions)
        {
            CollisionResult result;

            //circle to circle needs no broad phase
            if (moveable.Hitbox is CircleHitbox && other.Hitbox is CircleHitbox)
            {
                result = _circleCollisionDetector.CheckCollision(moveable, other);
                if (result.PenetrationDepth > 0)
                    collisions.Add(result);
                return;
            }

            // Broad phase: AABB check
            if (!_aabbCollisionDetector.CheckPotentialCollision(moveable, other))
                return;

            // Narrow phase: Dispatch to appropriate detector
            // If either hitbox is a circle, use circle detector                
            if (moveable.Hitbox is CircleHitbox || other.Hitbox is CircleHitbox)
                result = _circleCollisionDetector.CheckCollision(moveable, other);
            else
                result = _satCollisionDetector.CheckCollision(moveable, other);

            if (result.PenetrationDepth > 0)
                collisions.Add(result);
        }
    }
}
