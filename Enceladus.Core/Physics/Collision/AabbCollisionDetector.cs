using Enceladus.Core.Utils;
using Enceladus.Core.World;
using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;


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
            return cell.X >= aabbRect.MinX && cell.X <= aabbRect.MaxX &&
                cell.Y >= aabbRect.MinY && cell.Y <= aabbRect.MaxY;
        }

        public bool CheckPotentialCollision(ICollidableEntity entity1, ICollidableEntity entity2)
        {
            var aabb1 = _aabbCalculator.CalculateAabb(entity1);
            var aabb2 = _aabbCalculator.CalculateAabb(entity2);

            // Check if AABBs overlap
            return aabb1.MinX <= aabb2.MaxX && aabb1.MaxX >= aabb2.MinX &&
                   aabb1.MinY <= aabb2.MaxY && aabb1.MaxY >= aabb2.MinY;
        }
    }
}
