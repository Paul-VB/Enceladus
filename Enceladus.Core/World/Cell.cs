using Raylib_cs;

namespace Enceladus.Core.World
{
    public struct Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CellType CellType { get; set; }
        public int Health { get; set; }
        public readonly bool HasCollision => CellType.HasCollision;
    }

    public class CellType
    {
        public int Id { get; set; }

        //todo: if we do switch to one big papa sprite texture as suggested in sprite service, this would need to be a pair of vector2s and not a path
        public string SpritePath { get; set; }
        public int MaxHealth { get; set; } = 0;
        public bool HasCollision { get; set; }
    }
}
