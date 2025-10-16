namespace Enceladus.Core.World
{
    public static class ChunkMath
    {
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
    }
}
