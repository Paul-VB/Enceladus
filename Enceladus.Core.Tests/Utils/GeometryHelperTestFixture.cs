using Enceladus.Core.Utils;
using Raylib_cs;

namespace Enceladus.Core.Tests.Utils
{
    public class GeometryHelperTestFixture
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 10)]
        [InlineData(-3, -7)]
        public void GetCellBounds_ReturnsCorrect1x1Rectangle(int cellX, int cellY)
        {
            // Act
            var result = GeometryHelper.GetCellBounds(cellX, cellY);

            // Assert - Cells are always 1x1 at their coordinates
            Assert.Equal(cellX, result.X);
            Assert.Equal(cellY, result.Y);
            Assert.Equal(1f, result.Width);
            Assert.Equal(1f, result.Height);
        }

        [Theory]
        [InlineData(0f, 10f, 0f, 10f, 0f, 0f, 10f, 10f)]
        [InlineData(-5f, 5f, -3f, 7f, -5f, -3f, 10f, 10f)]
        [InlineData(100f, 200f, 50f, 150f, 100f, 50f, 100f, 100f)]
        [InlineData(40.25f, 60.75f, 10.5f, 30.25f, 40.25f, 10.5f, 20.5f, 19.75f)] // Fractional coords
        [InlineData(0f, 100f, 0f, 50f, 0f, 0f, 100f, 50f)] // Non-square rectangle
        [InlineData(-10.5f, -5.25f, -20.75f, -10.25f, -10.5f, -20.75f, 5.25f, 10.5f)] // Negative fractional
        public void RectangleFromBounds_CalculatesCorrectWidthAndHeight(float minX, float maxX, float minY, float maxY, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
        {
            // Act
            var result = GeometryHelper.RectangleFromBounds(minX, maxX, minY, maxY);

            // Assert
            Assert.Equal(expectedX, result.X);
            Assert.Equal(expectedY, result.Y);
            Assert.Equal(expectedWidth, result.Width);
            Assert.Equal(expectedHeight, result.Height);
        }

        [Fact]
        public void DoRectanglesOverlap_IdenticalRectangles_ReturnsTrue()
        {
            // Arrange
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(0, 0, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DoRectanglesOverlap_PartialOverlap_ReturnsTrue()
        {
            // Arrange
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(5, 5, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DoRectanglesOverlap_EdgeTouching_ReturnsFalse()
        {
            // Arrange
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(10, 0, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.False(result); // Edge touching doesn't count as overlap in strict AABB test
        }

        [Fact]
        public void DoRectanglesOverlap_NoOverlap_ReturnsFalse()
        {
            // Arrange
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(20, 20, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DoRectanglesOverlap_OneInsideOther_ReturnsTrue()
        {
            // Arrange
            var rect1 = new Rectangle(0, 0, 100, 100);
            var rect2 = new Rectangle(25, 25, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DoRectanglesOverlap_NegativeCoordinates_WorksCorrectly()
        {
            // Arrange
            var rect1 = new Rectangle(-10, -10, 20, 20);
            var rect2 = new Rectangle(-5, -5, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DoRectanglesOverlap_SlightOverlap_ReturnsTrue()
        {
            // Arrange - Rectangles overlapping by just 0.1 units
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(9.9f, 0, 10, 10);

            // Act
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Assert
            Assert.True(result);
        }
    }
}
