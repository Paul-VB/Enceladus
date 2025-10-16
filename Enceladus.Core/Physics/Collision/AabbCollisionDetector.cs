using Enceladus.Core.Entities;
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

            //todo: explain why we loop on each chunk instead of looping on cells?
            // Find all chunks that overlap this AABB
            var overlappingChunks = ChunkMath.GetChunksInBounds(map, aabbRect.MinX, aabbRect.MaxX, aabbRect.MinY, aabbRect.MaxY);

            foreach (var chunk in overlappingChunks)
            {
                foreach (var cell in chunk.Cells)
                {
                    if (cell.HasCollision && CellCollidesWithAabbRect(cell, aabbRect))
                    {
                        candidates.Add(cell);
                    }
                }
            }

            return candidates;
        }

        private bool CellCollidesWithAabbRect(Cell cell, AabbRectangle aabbRect)
        {
            // Cell is a 1x1 square: [cell.X, cell.X+1] x [cell.Y, cell.Y+1]
            // Create AABB for the cell and check overlap
            var cellRect = new AabbRectangle(new Rectangle(
                cell.X,     // x
                cell.Y,     // y
                1,          // width
                1           // height
            ));
            return DoAabbsOverlap(cellRect, aabbRect);
        }

        public bool CheckPotentialCollision(ICollidableEntity entity1, ICollidableEntity entity2)
        {
            var aabb1 = _aabbCalculator.CalculateAabb(entity1);
            var aabb2 = _aabbCalculator.CalculateAabb(entity2);

            return DoAabbsOverlap(aabb1, aabb2);
        }

        private bool DoAabbsOverlap(AabbRectangle rect1, AabbRectangle rect2)
        {
            return rect1.MinX <= rect2.MaxX && rect1.MaxX >= rect2.MinX &&
                   rect1.MinY <= rect2.MaxY && rect1.MaxY >= rect2.MinY;
        }
    }
}
