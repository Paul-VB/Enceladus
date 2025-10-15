using Enceladus.Core.Utils;

namespace Enceladus.Core.Tests.Utils
{
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

        [Fact]
        public void WorldToLocalCoords_AlwaysReturnsValidRange()
        {
            // Test a bunch of random coordinates to ensure local coords are always 0-15
            var random = new Random(42);
            for (int i = 0; i < 1000; i++)
            {
                int worldX = random.Next(-1000, 1000);
                int worldY = random.Next(-1000, 1000);

                var (localX, localY) = ChunkMath.WorldToLocalCoords(worldX, worldY);

                Assert.InRange(localX, 0, 15);
                Assert.InRange(localY, 0, 15);
            }
        }
    }
}
