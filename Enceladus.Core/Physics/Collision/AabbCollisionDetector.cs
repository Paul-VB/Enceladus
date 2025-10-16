using Enceladus.Core.Entities;
using Enceladus.Core.Utils;
using Enceladus.Core.World;
using Raylib_cs;


namespace Enceladus.Core.Physics.Collision
{
    public interface IAabbCollisionDetector
    {
        List<Cell> CheckPotentialCellCollisions(ICollidableEntity entity, Map map);
        bool CheckPotentialCollision(ICollidableEntity entity1, ICollidableEntity entity2);
    }
    public class AabbCollisionDetector : IAabbCollisionDetector
    {
        private readonly IAabbCalculator _aabbCalculator;
        public AabbCollisionDetector(IAabbCalculator aabbCalculator)
        {
            _aabbCalculator = aabbCalculator;
        }
        public List<Cell> CheckPotentialCellCollisions(ICollidableEntity entity, Map map)
        {
            var candidates = new List<Cell>();
            var aabbRect = _aabbCalculator.CalculateAabb(entity);

            // Get all cells that overlap the AABB bounds
            // ChunkMath.GetCellsInBounds handles the chunk iteration and cell filtering for us
            foreach (var cell in ChunkMath.GetCellsInBounds(map, aabbRect))
            {
                if (cell.HasCollision)
                {
                    candidates.Add(cell);
                }
            }

            return candidates;
        }

        public bool CheckPotentialCollision(ICollidableEntity entity1, ICollidableEntity entity2)
        {
            var aabb1 = _aabbCalculator.CalculateAabb(entity1);
            var aabb2 = _aabbCalculator.CalculateAabb(entity2);

            return GeometryHelper.DoRectanglesOverlap(aabb1, aabb2);
        }
    }
}
