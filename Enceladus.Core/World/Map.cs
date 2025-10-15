using Raylib_cs;

namespace Enceladus.Core.World
{
    public class Map
    {
        public Guid Id { get; set; }
        public Dictionary<(int, int), MapChunk> Chunks { get; set; } = [];

        // TODO: Add biome type, resources, enemy spawns, instability level, etc.
    }
}
