namespace Enceladus.Core.World
{
    public static class ChunkMath
    {
        //todo: debatable if we want this in config. its technically a magic number but i cannot see a benefit to changing this or tweaking it ever.
        public const int ChunkSize = 16;

        public static (int chunkX, int chunkY) WorldToChunkCoords(int worldX, int worldY)
        {
            // Use floor division to handle negative coords correctly
            int chunkX = worldX >= 0 ? worldX / ChunkSize : (worldX - ChunkSize + 1) / ChunkSize;
            int chunkY = worldY >= 0 ? worldY / ChunkSize : (worldY - ChunkSize + 1) / ChunkSize;
            return (chunkX, chunkY);
        }

        public static (int worldX, int worldY) ChunkToWorldCoords(int chunkX, int chunkY)
        {
            return (chunkX * ChunkSize, chunkY * ChunkSize);
        }

        public static (int localX, int localY) WorldToLocalCoords(int worldX, int worldY)
        {
            var (chunkX, chunkY) = WorldToChunkCoords(worldX, worldY);
            int localX = worldX - chunkX * ChunkSize;
            int localY = worldY - chunkY * ChunkSize;
            return (localX, localY);
        }

        /// <summary>
        /// Returns all chunks that overlap the given rectangular bounds in world space.
        /// </summary>
        public static List<MapChunk> GetChunksInBounds(Map map, float minX, float maxX, float minY, float maxY)
        {
            var chunks = new List<MapChunk>();

            // Convert world bounds to chunk coordinates
            var (minChunkX, minChunkY) = WorldToChunkCoords((int)minX, (int)minY);
            var (maxChunkX, maxChunkY) = WorldToChunkCoords((int)maxX, (int)maxY);

            // Iterate through chunk range and collect existing chunks
            for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
            {
                for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
                {
                    if (map.Chunks.TryGetValue((chunkX, chunkY), out var chunk))
                    {
                        chunks.Add(chunk);
                    }
                }
            }

            return chunks;
        }
    }
}
