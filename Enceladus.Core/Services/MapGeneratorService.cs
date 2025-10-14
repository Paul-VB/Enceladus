using Enceladus.Core.World;

namespace Enceladus.Core.Services
{
    public interface IMapGeneratorService
    {
        Map GenerateTestMap();
    }

    public class MapGeneratorService : IMapGeneratorService
    {
        public Map GenerateTestMap()
        {
            const int width = 100;
            const int height = 100;

            var map = new Map()
            {
                Width = width,
                Height = height,
                Cells = new Cell[width][]
            };

            // Fill entire map with water
            for (int x = 0; x < width; x++)
            {
                map.Cells[x] = new Cell[height];
                for (int y = 0; y < height; y++)
                {
                    map.Cells[x][y] = new Cell
                    {
                        CellType = CellTypes.Water,
                        Health = CellTypes.Water.MaxHealth
                    };
                }
            }

            // Add some ice patches for testing
            AddIcePatch(map, 30, 30, 10, 10);
            AddIcePatch(map, 60, 60, 15, 8);

            return map;
        }

        private void AddIcePatch(Map map, int startX, int startY, int width, int height)
        {
            for (int x = startX; x < Math.Min(startX + width, map.Width); x++)
            {
                for (int y = startY; y < Math.Min(startY + height, map.Height); y++)
                {
                    map.Cells[x][y] = new Cell
                    {
                        CellType = CellTypes.Ice,
                        Health = CellTypes.Ice.MaxHealth
                    };
                }
            }
        }
    }
}
