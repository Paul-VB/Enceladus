
using Enceladus.Core.Tests.Helpers;
using Enceladus.Core.World;

namespace Enceladus.Core.Tests.World
{
    //todo: (stretch ish goal) the tests are fine but we should refactor this to have a separate test fixture for each function were testing like we used to... but im getting bored of working on this test fixture. do it later lol
    public class ChunkMathTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0)]           // Origin
        [InlineData(15, 15, 0, 0)]         // Within first chunk
        [InlineData(16, 16, 1, 1)]         // Start of next chunk
        [InlineData(31, 31, 1, 1)]         // End of second chunk
        [InlineData(-1, -1, -1, -1)]       // Just before origin
        [InlineData(-16, -16, -1, -1)]     // Start of negative chunk
        [InlineData(-17, -17, -2, -2)]     // Second negative chunk
        [InlineData(100, -50, 6, -4)]      // Mixed positive/negative
        public void WorldToChunkCoords_ReturnsCorrectChunk(int worldX, int worldY, int expectedChunkX, int expectedChunkY)
        {
            // Act
            var (chunkX, chunkY) = ChunkMath.WorldToChunkCoords(worldX, worldY);

            // Assert
            Assert.Equal(expectedChunkX, chunkX);
            Assert.Equal(expectedChunkY, chunkY);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]           // Origin -> local (0, 0)
        [InlineData(15, 15, 15, 15)]       // Last cell in chunk 0
        [InlineData(16, 16, 0, 0)]         // First cell in chunk 1
        [InlineData(-1, -1, 15, 15)]       // Last cell in chunk -1
        [InlineData(-16, -16, 0, 0)]       // First cell in chunk -1
        [InlineData(31, 47, 15, 15)]       // Mixed coords
        public void WorldToLocalCoords_ReturnsCorrectLocalPosition(int worldX, int worldY, int expectedLocalX, int expectedLocalY)
        {
            // Act
            var (localX, localY) = ChunkMath.WorldToLocalCoords(worldX, worldY);

            // Assert
            Assert.Equal(expectedLocalX, localX);
            Assert.Equal(expectedLocalY, localY);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(500, -750)]
        [InlineData(-999, 888)]
        [InlineData(12345, -6789)]
        [InlineData(-5000, -5000)]
        [InlineData(777, 333)]
        [InlineData(-100, 200)]
        [InlineData(16384, -16384)]
        [InlineData(-1, -1)]
        [InlineData(31, 47)]
        public void WorldToLocalCoords_AlwaysReturnsValidRange(int worldX, int worldY)
        {
            // Act
            var (localX, localY) = ChunkMath.WorldToLocalCoords(worldX, worldY);

            // Assert - Local coords must always be in range [0, 15]
            Assert.InRange(localX, 0, 15);
            Assert.InRange(localY, 0, 15);
        }

        [Fact]
        public void GetChunksInBounds_ExactChunkBoundary_ReturnsSingleChunk()
        {
            // Arrange - Map with single chunk at (0, 0)
            var map = MapHelpers.CreateMapWithChunks((0, 0));
            var bounds = new Raylib_cs.Rectangle(0, 0, 16, 16); // Exact chunk size

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert
            Assert.Single(result);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
        }

        [Fact]
        public void GetChunksInBounds_PartialOverlapWithMultipleChunks_ReturnsAllOverlappingChunks()
        {
            // Arrange - Map with 4 chunks in 2x2 grid
            var map = MapHelpers.CreateMapWithChunks((0, 0), (1, 0), (0, 1), (1, 1));
            var bounds = new Raylib_cs.Rectangle(8, 8, 16, 16); // Overlaps all 4 chunks

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0);
            Assert.Contains(result, c => c.X == 0 && c.Y == 1);
            Assert.Contains(result, c => c.X == 1 && c.Y == 1);
        }

        [Fact]
        public void GetChunksInBounds_SlightOverlapIntoNextChunk_IncludesNextChunk()
        {
            // Arrange - This tests the Floor/Ceiling bug fix
            var map = MapHelpers.CreateMapWithChunks((0, 0), (1, 0));
            var bounds = new Raylib_cs.Rectangle(0, 0, 16.1f, 16); // Extends 0.1 into chunk (1, 0)

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert - Should include both chunks because bounds extends into second chunk
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0);
        }

        [Fact]
        public void GetChunksInBounds_NoOverlappingChunks_ReturnsEmpty()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithChunks((0, 0), (1, 1));
            var bounds = new Raylib_cs.Rectangle(100, 100, 10, 10); // Far away from any chunks

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetChunksInBounds_NegativeCoordinates_WorksCorrectly()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithChunks((-1, -1), (0, 0));
            var bounds = new Raylib_cs.Rectangle(-16, -16, 32, 32); // Covers both chunks

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == -1 && c.Y == -1);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
        }

        [Fact]
        public void GetChunksInBounds_NegativeFractionalCoordinates_UsesFloorNotTruncation()
        {
            // Arrange - This tests the Floor bug fix for negative coords
            // Without Floor, (int)(-0.5) = 0 (truncates toward zero), missing chunk -1
            // With Floor, Floor(-0.5) = -1 (correct)
            var map = MapHelpers.CreateMapWithChunks((-1, -1), (0, -1), (-1, 0), (0, 0));
            var bounds = new Raylib_cs.Rectangle(-0.5f, -0.5f, 1f, 1f); // Small rect from (-0.5, -0.5) to (0.5, 0.5)

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert - Should include all 4 chunks that the bounds touches
            Assert.Equal(4, result.Count);
            Assert.Contains(result, c => c.X == -1 && c.Y == -1);
            Assert.Contains(result, c => c.X == 0 && c.Y == -1);
            Assert.Contains(result, c => c.X == -1 && c.Y == 0);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
        }

        [Fact]
        public void GetChunksInBounds_PositiveFractionalMaxBounds_UsesCeilingNotTruncation()
        {
            // Arrange - This tests the Ceiling bug fix for maxX/maxY
            // Rectangle from (0, 0) with width 20.1 extends to x=20.1
            // Without Ceiling, (int)(20.1) = 20, WorldToChunkCoords(20) = chunk 1
            // But we need chunk containing x=20.1, which requires Ceiling(20.1) = 21
            var map = MapHelpers.CreateMapWithChunks((0, 0), (1, 0), (2, 0));
            var bounds = new Raylib_cs.Rectangle(0, 0, 20.1f, 16); // Extends 0.1 into chunk 2 (at x=32)

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert - Should include chunks 0, 1 (bounds goes from 0 to 20.1)
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0);
            Assert.DoesNotContain(result, c => c.X == 2); // Chunk 2 starts at x=32, bounds only goes to 20.1
        }

        [Fact]
        public void GetChunksInBounds_FractionalMaxBounds_XOverlap_UsesCeilingNotTruncation()
        {
            // Arrange - Rectangle from (10, 10) with size (5.1, 5) extends to (15.1, 15)
            // Chunk (0,0) contains cells 0-15, so 15.1 extends into chunk (1,0)
            // Without Ceiling, (int)(15.1) = 15, which misses the overlap into chunk (1,0)
            // With Ceiling, Ceiling(15.1) = 16, which correctly detects chunk (1,0) overlap
            var map = MapHelpers.CreateMapWithChunks((0, 0), (1, 0), (0, 1), (1, 1));
            var bounds = new Raylib_cs.Rectangle(10, 10, 5.1f, 5); // From (10,10) to (15.1, 15)

            // Act
            var result = ChunkMath.GetChunksInBounds(map, bounds).ToList();

            // Assert - Should include both chunks because x=15.1 extends fractionally into chunk (1,0)
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0); // x=15.1 extends into next chunk
            Assert.DoesNotContain(result, c => c.X == 0 && c.Y == 1); // y=15 stays within chunk (0,0)
        }

        [Fact]
        public void GetCellsInBounds_ReturnsOnlyCellsInBounds()
        {
            // Arrange - Create chunk with cells
            var map = MapHelpers.CreateMapWithCells(
                (0, 0), (1, 0), (2, 0), // First row
                (0, 1), (1, 1), (2, 1)  // Second row
            );
            var bounds = new Raylib_cs.Rectangle(0, 0, 2, 2); // Should include cells (0,0), (1,0), (0,1), (1,1)

            // Act
            var result = ChunkMath.GetCellsInBounds(map, bounds).ToList();

            // Assert
            Assert.Equal(4, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0);
            Assert.Contains(result, c => c.X == 0 && c.Y == 1);
            Assert.Contains(result, c => c.X == 1 && c.Y == 1);
            Assert.DoesNotContain(result, c => c.X == 2); // Cell at (2, y) should be excluded
        }

        [Fact]
        public void GetCellsInBounds_PartialCellOverlap_IncludesCell()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithCells((0, 0), (1, 0));
            var bounds = new Raylib_cs.Rectangle(0, 0, 1.1f, 1); // Extends 0.1 into cell (1, 0)

            // Act
            var result = ChunkMath.GetCellsInBounds(map, bounds).ToList();

            // Assert - Should include both cells
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.X == 0 && c.Y == 0);
            Assert.Contains(result, c => c.X == 1 && c.Y == 0);
        }

        [Fact]
        public void GetCellsInBounds_PartialCellOverlap_NegativeCoords_IncludesCell()
        {
            // Arrange
            var map = MapHelpers.CreateMapWithCells((-1, -1), (0, -1), (-1, 0));
            var bounds = new Raylib_cs.Rectangle(-1, -1, 1.1f, 1.1f); // Extends slightly into positive cells

            // Act
            var result = ChunkMath.GetCellsInBounds(map, bounds).ToList();

            // Assert - Should include all cells that partially overlap
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.X == -1 && c.Y == -1);
            Assert.Contains(result, c => c.X == 0 && c.Y == -1);
            Assert.Contains(result, c => c.X == -1 && c.Y == 0);
        }

    }
}
