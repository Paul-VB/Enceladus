using Enceladus.Core.Entities;
using Enceladus.Core.World;


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
            // Find chunk range that overlaps this AABB
            var (minChunkX, minChunkY) = ChunkMath.WorldToChunkCoords(aabbRect.MinX, aabbRect.MinY);
            var (maxChunkX, maxChunkY) = ChunkMath.WorldToChunkCoords(aabbRect.MaxX, aabbRect.MaxY);

            for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
            {
                for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
                {
                    if (map.Chunks.TryGetValue((chunkX, chunkY), out var chunk))
                    {
                        foreach (var cell in chunk.Cells)
                        {
                            if (cell.HasCollision && CellCollidesWithAabbRect(cell, aabbRect))
                            {
                                candidates.Add(cell);
                            }
                        }
                    }
                }
            }

            return candidates;
        }

        private bool CellCollidesWithAabbRect(Cell cell, AabbRectangle aabbRect)
        {
            //todo: this seems very similar to the code in check potential collisions. DRY, pull it out into a helpper func? also this code here
            //seems wrong. if these are in world coords (1 cell = 1.0 world units), then dont we need to do cell.X - 1 >= aabbRect.minX??
            return cell.X >= aabbRect.MinX && cell.X <= aabbRect.MaxX &&
                cell.Y >= aabbRect.MinY && cell.Y <= aabbRect.MaxY;
        }

        public bool CheckPotentialCollision(ICollidableEntity entity1, ICollidableEntity entity2)
        {
            var aabb1 = _aabbCalculator.CalculateAabb(entity1);
            var aabb2 = _aabbCalculator.CalculateAabb(entity2);

            //todo: maybe combine this with the above logic?
            // Check if AABBs overlap
            return aabb1.MinX <= aabb2.MaxX && aabb1.MaxX >= aabb2.MinX &&
                   aabb1.MinY <= aabb2.MaxY && aabb1.MaxY >= aabb2.MinY;
        }
    }
}
