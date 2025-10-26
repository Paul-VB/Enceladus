using Enceladus.Core.Utils;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class GetCellBoundsTestFixture
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(5, 10)]
        [InlineData(-3, -7)]
        public void GivenCellCoordinates_WhenGettingBounds_ThenReturnsCorrect1x1Rectangle(int cellX, int cellY)
        {
            // When
            var result = GeometryHelper.GetCellBounds(cellX, cellY);

            // Then - Cells are always 1x1 at their coordinates
            Assert.Equal(cellX, result.X);
            Assert.Equal(cellY, result.Y);
            Assert.Equal(1f, result.Width);
            Assert.Equal(1f, result.Height);
        }
    }
}
