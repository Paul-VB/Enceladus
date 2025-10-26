using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class CalculateSignedAreaTestFixture : GeometryHelperTestBase
    {
        private float CalculateSignedArea() => GeometryHelper.CalculateSignedArea(TestVertices);

        [Fact]
        public void GivenUnitSquareCounterClockwise_WhenCalculatingSignedArea_ThenReturnsNegative()
        {
            // In screen coordinates (Y down), counter-clockwise = negative
            GivenVertices((0, 0), (1, 0), (1, 1), (0, 1));

            var result = CalculateSignedArea();

            Assert.True(result < 0);
            Assert.Equal(-1f, result, precision: 5);
        }

        [Fact]
        public void GivenUnitSquareClockwise_WhenCalculatingSignedArea_ThenReturnsPositive()
        {
            // In screen coordinates (Y down), clockwise = positive
            GivenVertices((0, 0), (0, 1), (1, 1), (1, 0));

            var result = CalculateSignedArea();

            Assert.True(result > 0);
            Assert.Equal(1f, result, precision: 5);
        }

        [Fact]
        public void Given2x3RectangleCounterClockwise_WhenCalculatingSignedArea_ThenReturnsNegative6()
        {
            // Counter-clockwise in screen coords
            GivenVertices((0, 0), (2, 0), (2, 3), (0, 3));

            var result = CalculateSignedArea();

            Assert.Equal(-6f, result, precision: 5);
        }

        [Fact]
        public void GivenTriangleCounterClockwise_WhenCalculatingSignedArea_ThenReturnsNegative6()
        {
            // Counter-clockwise in screen coords
            GivenVertices((0, 0), (4, 0), (2, 3));

            var result = CalculateSignedArea();

            Assert.Equal(-6f, result, precision: 5);
        }

        [Fact]
        public void GivenEmptyList_WhenCalculatingSignedArea_ThenReturnsZero()
        {
            TestVertices = new List<Vector2>();

            var result = CalculateSignedArea();

            Assert.Equal(0f, result);
        }

        [Fact]
        public void GivenNullList_WhenCalculatingSignedArea_ThenReturnsZero()
        {
            TestVertices = null;

            var result = CalculateSignedArea();

            Assert.Equal(0f, result);
        }

        [Fact]
        public void GivenTwoVertices_WhenCalculatingSignedArea_ThenReturnsZero()
        {
            GivenVertices((0, 0), (1, 1));

            var result = CalculateSignedArea();

            Assert.Equal(0f, result);
        }

        [Fact]
        public void GivenLShapeConcavePolygonCounterClockwise_WhenCalculatingSignedArea_ThenReturnsNegative3()
        {
            // Counter-clockwise in screen coords
            GivenVertices((0, 0), (2, 0), (2, 1), (1, 1), (1, 2), (0, 2));

            var result = CalculateSignedArea();

            Assert.Equal(-3f, result, precision: 5);
        }

        [Fact]
        public void GivenCenteredSquareCounterClockwise_WhenCalculatingSignedArea_ThenReturnsNegative4()
        {
            // Counter-clockwise in screen coords
            GivenVertices((-1, -1), (1, -1), (1, 1), (-1, 1));

            var result = CalculateSignedArea();

            Assert.Equal(-4f, result, precision: 5);
        }

        [Fact]
        public void GivenRegularHexagonCounterClockwise_WhenCalculatingSignedArea_ThenReturnsNegativeArea()
        {
            // Regular hexagon with radius 1, counter-clockwise in screen coords
            GivenVertices(
                (1f, 0f),
                (0.5f, 0.866f),
                (-0.5f, 0.866f),
                (-1f, 0f),
                (-0.5f, -0.866f),
                (0.5f, -0.866f)
            );

            var result = CalculateSignedArea();

            // Regular hexagon area = (3 * sqrt(3) / 2) * r^2 ≈ 2.598
            Assert.Equal(-2.598f, result, precision: 2);
        }

        [Fact]
        public void GivenSameShapeDifferentStartVertex_WhenCalculatingSignedArea_ThenReturnsSameArea()
        {
            GivenVertices((0, 0), (1, 0), (1, 1), (0, 1));
            var area1 = CalculateSignedArea();

            GivenVertices((1, 0), (1, 1), (0, 1), (0, 0));
            var area2 = CalculateSignedArea();

            Assert.Equal(area1, area2, precision: 5);
        }

        [Fact]
        public void GivenReversedVertices_WhenCalculatingSignedArea_ThenReturnsOppositeSign()
        {
            GivenVertices((0, 0), (1, 0), (1, 1), (0, 1));
            var area1 = CalculateSignedArea();

            GivenVertices((0, 1), (1, 1), (1, 0), (0, 0));
            var area2 = CalculateSignedArea();

            Assert.Equal(-area1, area2, precision: 5);
        }
    }
}
