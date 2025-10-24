using Enceladus.Core.Config;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Utils;
using Raylib_cs;

namespace Enceladus.Core.World
{
    public static class ChunkMath
    {

        public static (int chunkX, int chunkY) WorldToChunkCoords(int worldX, int worldY)
        {
            // Use floor division to handle negative coords correctly
            int chunkX = worldX >= 0 ? worldX / Constants.ChunkSize : (worldX - Constants.ChunkSize + 1) / Constants.ChunkSize;
            int chunkY = worldY >= 0 ? worldY / Constants.ChunkSize : (worldY - Constants.ChunkSize + 1) / Constants.ChunkSize;
            return (chunkX, chunkY);
        }

        public static (int worldX, int worldY) ChunkToWorldCoords(int chunkX, int chunkY)
        {
            return (chunkX * Constants.ChunkSize, chunkY * Constants.ChunkSize);
        }

        public static (int localX, int localY) WorldToLocalCoords(int worldX, int worldY)
        {
            var (chunkX, chunkY) = WorldToChunkCoords(worldX, worldY);
            int localX = worldX - chunkX * Constants.ChunkSize;
            int localY = worldY - chunkY * Constants.ChunkSize;
            return (localX, localY);
        }

        public static IEnumerable<MapChunk> GetChunksInBounds(Map map, Rectangle bounds)
        {
            float minX = bounds.X;
            float maxX = bounds.X + bounds.Width;
            float minY = bounds.Y;
            float maxY = bounds.Y + bounds.Height;

            var (minChunkX, minChunkY) = WorldToChunkCoords((int)MathF.Floor(minX), (int)MathF.Floor(minY));
            var (maxChunkX, maxChunkY) = WorldToChunkCoords((int)MathF.Ceiling(maxX), (int)MathF.Ceiling(maxY));

            // Iterate through chunk range and yield existing chunks
            for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
            {
                for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
                {
                    if (map.Chunks.TryGetValue((chunkX, chunkY), out var chunk))
                    {
                        yield return chunk;
                    }
                }
            }
        }

        public static IEnumerable<Cell> GetCellsInBounds(Map map, Rectangle bounds)
        {
            var chunks = GetChunksInBounds(map, bounds);

            foreach (var chunk in chunks)
            {
                foreach (var cell in chunk.Cells)
                {
                    // Check if cell's 1x1 bounds overlap with the query bounds
                    var cellBounds = GeometryHelper.GetCellBounds(cell.X, cell.Y);
                    if (GeometryHelper.DoRectanglesOverlap(cellBounds, bounds))
                    {
                        yield return cell;
                    }
                }
            }
        }
    }
}
