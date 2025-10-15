namespace Enceladus.Core.World
{
    public class MapChunk
    {
        public int X { get; }
        public int Y { get; }

        public List<Cell> Cells { get; set; } = [];

        public MapChunk(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
