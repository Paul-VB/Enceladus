using Enceladus.Core.Entities;
using Enceladus.Core.Utils;
using Enceladus.Core.World;


namespace Enceladus.Core.Physics.Collision.Detection
{
    public interface IAabbCollisionDetector
    {
        List<Cell> CheckPotentialCellCollisions(Entity entity, Map map);
        bool CheckPotentialCollision(Entity entity, ICollidable otherObject);
    }
    public class AabbCollisionDetector : IAabbCollisionDetector
    {
        private readonly IAabbCalculator _aabbCalculator;
        public AabbCollisionDetector(IAabbCalculator aabbCalculator)
        {
            _aabbCalculator = aabbCalculator;
        }
        public List<Cell> CheckPotentialCellCollisions(Entity entity, Map map)
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

        public bool CheckPotentialCollision(Entity entity, ICollidable otherObject)
        {
            var aabb1 = _aabbCalculator.CalculateAabb(entity);
            var aabb2 = _aabbCalculator.CalculateAabb(otherObject);

            return GeometryHelper.DoRectanglesOverlap(aabb1, aabb2);
        }
    }
}
