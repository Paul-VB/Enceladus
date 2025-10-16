using Raylib_cs;

namespace Enceladus.Core.World
{
    public class Map
    {
        public Guid Id { get; set; }
        public Dictionary<(int, int), MapChunk> Chunks { get; set; } = [];

        //todo: if we implement map saving and loading, we would want a list of entities that live on the map like monsters and such. not sure how we handle the player going from sector to sector?

        // TODO: Add biome type, resources, enemy spawns, instability level, etc.
    }
}
