using Enceladus.Core.Physics.Hitboxes;

namespace Enceladus.Core.World.Chunks
{
    public interface IChunkHitboxGenerator
    {
        /// <summary>
        /// Generates merged hitboxes for a chunk from its cells.
        /// Uses flood fill to find connected ice islands, marching squares to trace outlines.
        /// Returns a list of concave polygon hitboxes (one per island).
        /// </summary>
        List<ConcavePolygonHitbox> GenerateHitboxes(MapChunk chunk);
    }

    public class ChunkHitboxGenerator : IChunkHitboxGenerator
    {
        private readonly ISolidCellIslandFinder _solidCellIslandFinder;

        public ChunkHitboxGenerator(ISolidCellIslandFinder solidCellIslandFinder)
        {
            _solidCellIslandFinder = solidCellIslandFinder;
        }

        public List<ConcavePolygonHitbox> GenerateHitboxes(MapChunk chunk)
        {
            var hitboxes = new List<ConcavePolygonHitbox>();

            // Find all solid cell islands using flood fill
            var islands = _solidCellIslandFinder.FindIslands(chunk);

            // TODO: Implement marching squares to trace outlines for each island
            // TODO: Create ConcavePolygonHitbox for each island

            return hitboxes;
        }
    }
}
