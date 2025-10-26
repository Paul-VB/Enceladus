using Enceladus.Core.Utils;
using Raylib_cs;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class DoRectanglesOverlapTestFixture
    {
        [Fact]
        public void GivenIdenticalRectangles_WhenCheckingOverlap_ThenReturnsTrue()
        {
            // Given
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(0, 0, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenPartialOverlap_WhenCheckingOverlap_ThenReturnsTrue()
        {
            // Given
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(5, 5, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenEdgeTouching_WhenCheckingOverlap_ThenReturnsFalse()
        {
            // Given
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(10, 0, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then - Edge touching doesn't count as overlap in strict AABB test
            Assert.False(result);
        }

        [Fact]
        public void GivenNoOverlap_WhenCheckingOverlap_ThenReturnsFalse()
        {
            // Given
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(20, 20, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenOneRectangleInsideOther_WhenCheckingOverlap_ThenReturnsTrue()
        {
            // Given
            var rect1 = new Rectangle(0, 0, 100, 100);
            var rect2 = new Rectangle(25, 25, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenNegativeCoordinates_WhenCheckingOverlap_ThenWorksCorrectly()
        {
            // Given
            var rect1 = new Rectangle(-10, -10, 20, 20);
            var rect2 = new Rectangle(-5, -5, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenSlightOverlap_WhenCheckingOverlap_ThenReturnsTrue()
        {
            // Given - Rectangles overlapping by just 0.1 units
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(9.9f, 0, 10, 10);

            // When
            var result = GeometryHelper.DoRectanglesOverlap(rect1, rect2);

            // Then
            Assert.True(result);
        }
    }
}
