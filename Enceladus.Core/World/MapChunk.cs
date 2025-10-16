namespace Enceladus.Core.World
{
    //todo: should this be a struct? im still fuzzy on the performance benefits of structs vs classes
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
