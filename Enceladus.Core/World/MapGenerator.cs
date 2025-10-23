namespace Enceladus.Core.World
{
    public interface IMapGenerator
    {
        Map GenerateTestMap();
    }

    //todo: stretch goal, create true proc world gen using seeds and such. id like to learn more about how world gen actually works. for now, crappy random world gen works for testing i suppose
    public class MapGenerator : IMapGenerator
    {
        private readonly ICellFactory _cellFactory;

        public MapGenerator(ICellFactory cellFactory)
        {
            _cellFactory = cellFactory;
        }
        public Map GenerateTestMap()
        {
            // 1000x1000 world = 62.5 chunks (round up to 63 chunks = 1008x1008 world units)
            const int chunksWide = 63;
            const int chunksHigh = 63;
            const int halfWidth = chunksWide / 2;
            const int halfHeight = chunksHigh / 2;

            var map = new Map();

            for (int chunkX = -halfWidth; chunkX <= halfWidth; chunkX++)
            {
                for (int chunkY = -halfHeight; chunkY <= halfHeight; chunkY++)
                {
                    var chunk = new MapChunk(chunkX, chunkY);
                    map.Chunks[(chunkX, chunkY)] = chunk;
                }
            }

            // Add randomly dispersed ice blocks
            var random = new Random(42); // Seed for consistency
            int iceBlockCount = 500; // Number of random ice blocks

            for (int i = 0; i < iceBlockCount; i++)
            {
                int x = random.Next(-500, 500);
                int y = random.Next(-500, 500);
                int width = random.Next(3, 12);
                int height = random.Next(3, 12);

                AddIcePatch(map, x, y, width, height);
            }

            return map;
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
                        var cell = _cellFactory.CreateCell(CellTypes.Ice, x, y);
                        chunk.Cells.Add(cell);
                    }
                }
            }
        }
    }
}
