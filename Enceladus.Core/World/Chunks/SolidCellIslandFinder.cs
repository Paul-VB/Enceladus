using Enceladus.Core.Config;

namespace Enceladus.Core.World.Chunks
{
    // NOTE: This class is NOT currently used in the runtime collision system.
    //
    // Original Purpose: Part of a "merged chunk hitbox" optimization to fix the "wrong corner bounce"
    // problem where bullets hitting cell corners would get conflicting collision normals from adjacent cells.
    //
    // The plan was:
    //   1. Use this class to find connected solid cell "islands" via flood fill
    //   2. Use IslandOutlineTracer to trace the outline of each island
    //   3. Triangulate the outline to create merged hitboxes
    //   4. Replace individual cell hitboxes with merged polygonal hitboxes
    //
    // Current Status: DELAYED - Implementation was ~80% complete but we decided to solve the corner bounce
    // problem with a simpler approach: batching multiple collisions per entity before resolution, which
    // combines collision normals mathematically rather than geometrically merging hitboxes.
    //
    // This Code: Fully implemented and tested (see SolidCellIslandFinderTestFixture). Kept for potential
    // future use incase we want to revisit the merged hitbox approach.
    public interface ISolidCellIslandFinder
    {
        /// <summary>
        /// Finds all connected solid cell islands in a chunk using flood fill.
        /// Returns a list of islands, where each island is a list of cell coordinates.
        /// </summary>
        List<Island> FindIslands(MapChunk chunk);
    }

    public class SolidCellIslandFinder : ISolidCellIslandFinder
    {
        public List<Island> FindIslands(MapChunk chunk)
        {
            var islands = new List<Island>();
            var visited = new HashSet<(int, int)>();

            for (int x = 0; x < Constants.ChunkSize; x++)
            {
                for (int y = 0; y < Constants.ChunkSize; y++)
                {
                    var island = new Island();
                    FloodFill(chunk, x, y, visited, island);
                    if (island.Count > 0)
                        islands.Add(island);
                }
            }

            return islands;
        }

        private void FloodFill(MapChunk chunk, int cellX, int cellY, HashSet<(int, int)> visited, Island island)
        {
            if (cellX < 0 || cellY < 0 || cellX >= Constants.ChunkSize || cellY >= Constants.ChunkSize)
                return;

            if (!IsCellUnvisitedAndSolid(chunk, cellX, cellY, visited))
                return;

            island.Add((cellX, cellY));

            FloodFill(chunk, cellX + 1, cellY, visited, island);
            FloodFill(chunk, cellX - 1, cellY, visited, island);
            FloodFill(chunk, cellX, cellY + 1, visited, island);
            FloodFill(chunk, cellX, cellY - 1, visited, island);
        }

        private bool IsCellUnvisitedAndSolid(MapChunk chunk, int cellX, int cellY, HashSet<(int, int)> visited)
        {
            if (visited.Contains((cellX, cellY)))
                return false;

            // Mark visited FIRST to prevent redundant lookups from other branches
            visited.Add((cellX, cellY));

            var cell = chunk.Cells.FirstOrDefault(cell => cell.X == cellX && cell.Y == cellY);
            if (cell.Equals(default(Cell)) || !cell.HasCollision)
                return false;

            return true;
        }
    }

    // For this particular usecase, a hashmap is faster than a List<(int, int)>
    public class Island : HashSet<(int x, int y)> { }
}
