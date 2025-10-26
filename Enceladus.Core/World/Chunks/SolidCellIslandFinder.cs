using Enceladus.Core.Config;

namespace Enceladus.Core.World.Chunks
{
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

    public class Island : List<(int x, int y)> { }
}
