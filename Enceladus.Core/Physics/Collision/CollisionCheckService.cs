using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using Enceladus.Core.World;
using Enceladus.Entities;
using System.Numerics;

namespace Enceladus.Core.Physics.Collision
{
    public interface ICollisionCheckService
    {
        List<EntityToCellCollisionResult> CheckEntitiesToCells(List<ICollidableEntity> entities, Map map);
        List<BaseCollisionResult> CheckEntitiesToEntities(List<ICollidableEntity> entities);
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
            var collisions = new List<EntityToCellCollisionResult>();
            foreach (var entity in entities)
            {
                collisions.AddRange(CheckEntityToCells(entity, map));
            }
            return collisions;
        }

        private List<EntityToCellCollisionResult> CheckEntityToCells(ICollidableEntity entity, Map map)
        {
            var collisions = new List<EntityToCellCollisionResult>();

            // Broad check (AABB)
            var cellCollisionCandiates = _aabbCollisionDetector.CheckPotentialCellCollisions(entity, map);
            if (cellCollisionCandiates.Count == 0) return collisions;

            //temp placeholder
            var placeHolderCircleAlgo= (ICollidableEntity e, Cell c) => { return false; };

            // Narrow check
            Func<ICollidableEntity, Cell, bool> narrowCollisionAlgorithm =
                entity.Hitbox is RectHitbox || entity.Hitbox is PolygonHitbox ? _satCollisionDetector.CheckCollision : placeHolderCircleAlgo;

            foreach (var cell in cellCollisionCandiates)
            {
                var didCollide = narrowCollisionAlgorithm(entity, cell);
                if (didCollide)
                {
                    collisions.Add(new EntityToCellCollisionResult
                    {
                        Entity = entity,
                        Cell = cell
                    });
                }
            }

            return collisions;
        }

        public List<BaseCollisionResult> CheckEntitiesToEntities(List<ICollidableEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
