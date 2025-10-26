using Enceladus.Core.Utils;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class RectangleFromBoundsTestFixture
    {
        [Theory]
        [InlineData(0f, 10f, 0f, 10f, 0f, 0f, 10f, 10f)]
        [InlineData(-5f, 5f, -3f, 7f, -5f, -3f, 10f, 10f)]
        [InlineData(100f, 200f, 50f, 150f, 100f, 50f, 100f, 100f)]
        [InlineData(40.25f, 60.75f, 10.5f, 30.25f, 40.25f, 10.5f, 20.5f, 19.75f)]
        [InlineData(0f, 100f, 0f, 50f, 0f, 0f, 100f, 50f)]
        [InlineData(-10.5f, -5.25f, -20.75f, -10.25f, -10.5f, -20.75f, 5.25f, 10.5f)]
        public void GivenMinMaxBounds_WhenCreatingRectangle_ThenCalculatesCorrectWidthAndHeight(
            float minX, float maxX, float minY, float maxY,
            float expectedX, float expectedY, float expectedWidth, float expectedHeight)
        {
            // When
            var result = GeometryHelper.RectangleFromBounds(minX, maxX, minY, maxY);

            // Then
            Assert.Equal(expectedX, result.X);
            Assert.Equal(expectedY, result.Y);
            Assert.Equal(expectedWidth, result.Width);
            Assert.Equal(expectedHeight, result.Height);
        }
    }
}
