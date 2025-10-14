using Raylib_cs;

namespace Enceladus.Core.World
{
    public class Map
    {
        public Guid Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Cell[][] Cells { get; set; }

        // TODO: Add biome type, resources, enemy spawns, instability level, etc.
    }
}
