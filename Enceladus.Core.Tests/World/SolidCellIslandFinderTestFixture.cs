using Enceladus.Core.Tests.Helpers;
using Enceladus.Core.World;
using Enceladus.Core.World.Chunks;

namespace Enceladus.Core.Tests.World
{
    public class SolidCellIslandFinderTestFixture
    {
        private readonly ISolidCellIslandFinder _solidCellIslandFinder;

        private MapChunk _testMapChunk = new MapChunk(0, 0);

        public SolidCellIslandFinderTestFixture()
        {
            _solidCellIslandFinder = new SolidCellIslandFinder(); 
        }

        private Cell GivenACell(int x, int y, bool hasCollision = true)
        {
            var cell = CellHelpers.GivenACell(x, y, hasCollision);
            _testMapChunk.Cells.Add(cell);
            return cell;
        }

        private List<Island> FindIslands()
        {
            return _solidCellIslandFinder.FindIslands(_testMapChunk);
        }

        [Fact]
        public void GivenEmptyChunk_WhenFindingIslands_ThenReturnsNoIslands()
        {
            // Given

            // When
            var islands = FindIslands();

            // Then
            Assert.Empty(islands);
        }

        [Fact]
        public void GivenSingleCell_WhenFindingIslands_ThenReturnsSingleIslandWithOneCell()
        {
            // Given

            GivenACell(5, 5);

            // When
            var islands = FindIslands();

            // Then
            Assert.Single(islands);
            Assert.Single(islands[0]);
            Assert.Contains((5, 5), islands[0]);
        }

        [Fact]
        public void GivenSingleNonCollidingCell_WhenFindingIslands_ThenReturnsNoIslands()
        {
            // Given
            GivenACell(5, 5, hasCollision: false);

            // When
            var islands = FindIslands();

            // Then
            Assert.Empty(islands);
        }

        [Fact]
        public void GivenHorizontalLineOf4Cells_WhenFindingIslands_ThenReturnsSingleIslandWith4Cells()
        {
            // Given
            GivenACell(5, 5);
            GivenACell(6, 5);
            GivenACell(7, 5);
            GivenACell(8, 5);

            // When
            var islands = FindIslands();

            // Then
            Assert.Single(islands);
            Assert.Equal(4, islands[0].Count);
            Assert.Contains((5, 5), islands[0]);
            Assert.Contains((6, 5), islands[0]);
            Assert.Contains((7, 5), islands[0]);
            Assert.Contains((8, 5), islands[0]);
        }

        [Fact]
        public void Given2x2Square_WhenFindingIslands_ThenReturnsSingleIslandWith4Cells()
        {
            // Given
            GivenACell(5, 5);
            GivenACell(6, 5);
            GivenACell(5, 6);
            GivenACell(6, 6);

            // When
            var islands = FindIslands();

            // Then
            Assert.Single(islands);
            Assert.Equal(4, islands[0].Count);
        }

        [Fact]
        public void GivenTwoDisconnectedCells_WhenFindingIslands_ThenReturnsTwoIslands()
        {
            // Given
            GivenACell(0, 0);
            GivenACell(10, 10);

            // When
            var islands = FindIslands();

            // Then
            Assert.Equal(2, islands.Count);
            Assert.Single(islands[0]);
            Assert.Single(islands[1]);
        }

        [Fact]
        public void GivenLShapedIsland_WhenFindingIslands_ThenReturnsSingleIslandWith5Cells()
        {
            // Given - L-shape:
            // X X X
            // X
            // X
            GivenACell(0, 0);
            GivenACell(1, 0);
            GivenACell(2, 0);
            GivenACell(0, 1);
            GivenACell(0, 2);

            // When
            var islands = FindIslands();

            // Then
            Assert.Single(islands);
            Assert.Equal(5, islands[0].Count);
        }

        [Fact]
        public void GivenDonutShape_WhenFindingIslands_ThenReturnsSingleIslandWith12Cells()
        {
            // Given - Donut shape (flood fill doesn't care about holes):
            // X X X X
            // X     X
            // X     X
            // X X X X
            // Top row
            GivenACell(0, 0);
            GivenACell(1, 0);
            GivenACell(2, 0);
            GivenACell(3, 0);
            // Left column
            GivenACell(0, 1);
            GivenACell(0, 2);
            // Right column
            GivenACell(3, 1);
            GivenACell(3, 2);
            // Bottom row
            GivenACell(0, 3);
            GivenACell(1, 3);
            GivenACell(2, 3);
            GivenACell(3, 3);

            // When
            var islands = FindIslands();

            // Then
            Assert.Single(islands);
            Assert.Equal(12, islands[0].Count);
        }

        [Fact]
        public void GivenThreeDisconnectedIslands_WhenFindingIslands_ThenReturnsThreeIslands()
        {
            // Given
            // Island 1: single cell
            GivenACell(0, 0);
            // Island 2: horizontal pair
            GivenACell(5, 5);
            GivenACell(6, 5);
            // Island 3: 2x2 square
            GivenACell(10, 10);
            GivenACell(11, 10);
            GivenACell(10, 11);
            GivenACell(11, 11);

            // When
            var islands = FindIslands();

            // Then
            Assert.Equal(3, islands.Count);
            Assert.Contains(islands, i => i.Count == 1);
            Assert.Contains(islands, i => i.Count == 2);
            Assert.Contains(islands, i => i.Count == 4);
        }

        [Fact]
        public void GivenDiagonalCells_WhenFindingIslands_ThenReturnsSeparateIslands()
        {
            // Given - Diagonal cells should NOT connect (only 4-directional connectivity):
            // X
            //   X
            GivenACell(0, 0);
            GivenACell(1, 1);

            // When
            var islands = FindIslands();

            // Then
            Assert.Equal(2, islands.Count);
        }

        [Fact]
        public void GivenCellsAtChunkBoundary_WhenFindingIslands_ThenHandlesCorrectly()
        {
            // Given - Cell at edge of chunk (15, 15) is last valid position
            GivenACell(15, 15);
            GivenACell(14, 15);
            GivenACell(15, 14);

            // When
            var islands = FindIslands();

            // Then
            Assert.Single(islands);
            Assert.Equal(3, islands[0].Count);
        }

        [Fact]
        public void GivenMixOfCollidingAndNonCollidingCells_WhenFindingIslands_ThenOnlyIncludesColliding()
        {
            // Given
            GivenACell(0, 0, hasCollision: true);
            GivenACell(1, 0, hasCollision: false); // Water - should not connect
            GivenACell(2, 0, hasCollision: true);

            // When
            var islands = FindIslands();

            // Then
            Assert.Equal(2, islands.Count);
            Assert.All(islands, island => Assert.Single(island));
        }
    }
}
