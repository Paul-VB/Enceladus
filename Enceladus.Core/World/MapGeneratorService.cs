using Enceladus.Core.Utils;

namespace Enceladus.Core.World
{
    public interface IMapGeneratorService
    {
        Map GenerateTestMap();
    }

    public class MapGeneratorService : IMapGeneratorService
    {
        public Map GenerateTestMap()
        {
            const int chunksWide = 6;
            const int chunksHigh = 6;
            const int halfWidth = chunksWide / 2;
            const int halfHeight = chunksHigh / 2;

            var map = new Map();

            for (int chunkX = -halfWidth; chunkX < halfWidth; chunkX++)
            {
                for (int chunkY = -halfHeight; chunkY < halfHeight; chunkY++)
                {
                    var chunk = GenerateChunk(chunkX, chunkY);
                    map.Chunks[(chunkX, chunkY)] = chunk;
                }
            }

            // Add some ice patches for testing
            AddIcePatch(map, -10, -10, 10, 10);
            AddIcePatch(map, 5, 5, 15, 8);

            return map;
        }

        private MapChunk GenerateChunk(int chunkX, int chunkY)
        {
            var chunk = new MapChunk(chunkX, chunkY);

            // Generate 16x16 cells for this chunk
            for (int localX = 0; localX < ChunkMath.ChunkSize; localX++)
            {
                for (int localY = 0; localY < ChunkMath.ChunkSize; localY++)
                {
                    // Calculate world position
                    int worldX = chunkX * ChunkMath.ChunkSize + localX;
                    int worldY = chunkY * ChunkMath.ChunkSize + localY;

                    var cell = new Cell
                    {
                        X = worldX,
                        Y = worldY,
                        CellType = CellTypes.Water,
                        Health = CellTypes.Water.MaxHealth
                    };

                    chunk.Cells.Add(cell);
                }
            }

            return chunk;
        }

        private void AddIcePatch(Map map, int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    // Find which chunk this cell belongs to
                    var (chunkX, chunkY) = ChunkMath.WorldToChunkCoords(x, y);

                    if (map.Chunks.TryGetValue((chunkX, chunkY), out var chunk))
                    {
                        // Find the cell in the chunk's list
                        var (localX, localY) = ChunkMath.WorldToLocalCoords(x, y);
                        int cellIndex = localY * ChunkMath.ChunkSize + localX;

                        if (cellIndex >= 0 && cellIndex < chunk.Cells.Count)
                        {
                            var cell = chunk.Cells[cellIndex];
                            cell.CellType = CellTypes.Ice;
                            cell.Health = CellTypes.Ice.MaxHealth;
                            chunk.Cells[cellIndex] = cell;
                        }
                    }
                }
            }
        }
    }
}
