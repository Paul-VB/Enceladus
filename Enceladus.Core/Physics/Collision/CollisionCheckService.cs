using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.World;
using System.Collections.Concurrent;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionCheckService
    {
        List<EntityToCellCollisionResult> CheckEntitiesToCells(List<ICollidableEntity> entities, Map map);
        List<EntityToEntityCollisionResult> CheckEntitiesToEntities(List<ICollidableEntity> entities);
    }

    public class CollisionCheckService : ICollisionCheckService
    {
        private readonly IAabbCollisionDetector _aabbCollisionDetector;
        private readonly ISatCollisionDetector _satCollisionDetector;

        public CollisionCheckService(IAabbCollisionDetector aabbCollisionDetector, ISatCollisionDetector satCollisionDetector)
        {
            _aabbCollisionDetector = aabbCollisionDetector;
            _satCollisionDetector = satCollisionDetector;
        }

        public List<EntityToCellCollisionResult> CheckEntitiesToCells(List<ICollidableEntity> entities, Map map)
        {
            var collisions = new ConcurrentBag<EntityToCellCollisionResult>();

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

        private List<EntityToCellCollisionResult> CheckEntityToCells(ICollidableEntity entity, Map map)
        {
            var collisions = new List<EntityToCellCollisionResult>();

            // Broad check (AABB)
            var cellCollisionCandiates = _aabbCollisionDetector.CheckPotentialCellCollisions(entity, map);
            if (cellCollisionCandiates.Count == 0) return collisions;

            //temp placeholder
            var placeHolderCircleAlgo = (ICollidableEntity e, Cell c) =>
            {
                return new EntityToCellCollisionResult
                {
                    Entity = e,
                    Cell = c,
                    PenetrationDepth = 0,
                    CollisionNormal = System.Numerics.Vector2.Zero
                };
            };

            // Narrow check
            Func<ICollidableEntity, Cell, EntityToCellCollisionResult> narrowCollisionAlgorithm =
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

        public List<EntityToEntityCollisionResult> CheckEntitiesToEntities(List<ICollidableEntity> entities)
        {
            var collisions = new List<EntityToEntityCollisionResult>();

            // Check each unique pair of entities
            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    var entity1 = entities[i];
                    var entity2 = entities[j];

                    // Broad phase: AABB check
                    if (!_aabbCollisionDetector.CheckPotentialCollision(entity1, entity2))
                        continue;

                    // Narrow phase: For now, only check polygon-to-polygon collisions
                    if ((entity1.Hitbox is RectHitbox || entity1.Hitbox is PolygonHitbox) &&
                        (entity2.Hitbox is RectHitbox || entity2.Hitbox is PolygonHitbox))
                    {
                        var result = _satCollisionDetector.CheckCollision(entity1, entity2);
                        if (result.PenetrationDepth > 0)
                        {
                            collisions.Add(result);
                        }
                    }
                }
            }

            return collisions;
        }
    }
}
