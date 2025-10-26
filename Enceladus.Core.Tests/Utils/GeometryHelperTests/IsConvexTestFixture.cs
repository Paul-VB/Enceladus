using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class IsConvexTestFixture : GeometryHelperTestBase
    {
        private bool IsConvex() => GeometryHelper.IsConvex(TestVertices);


        [Fact]
        public void GivenSquare_WhenCheckingConvexity_ThenReturnsTrue()
        {
            //Unit square centered at origin
            GivenVertices((-.5f, -.5f), (0.5f, -0.5f), (0.5f, 0.5f), (-0.5f, 0.5f));

            var result = IsConvex();

            Assert.True(result);
        }

        [Fact]
        public void GivenTriangle_WhenCheckingConvexity_ThenReturnsTrue()
        {
            GivenVertices((0,0), (1,0), (0.5f,1));

            var result = IsConvex();

            Assert.True(result);
        }

        [Fact]
        public void GivenRegularPentagon_WhenCheckingConvexity_ThenReturnsTrue()
        {
            GivenVertices((0f, -1f), (0.951f, -0.309f), (0.588f, 0.809f), (-0.588f, 0.809f), (-0.951f, -0.309f));

            var result = IsConvex();

            Assert.True(result);
        }

        [Fact]
        public void GivenLShape_WhenCheckingConvexity_ThenReturnsFalse()
        {
            GivenVertices((0, 0), (2, 0), (2, 1), (1, 1), (1, 2), (0, 2));

            var result = IsConvex();

            Assert.False(result);
        }

        [Fact]
        public void GivenStarShape_WhenCheckingConvexity_ThenReturnsFalse()
        {
            GivenVertices((0, -1), (0.2f, -0.2f), (1, 0), (0.2f, 0.2f), (0, 1), (-0.2f, 0.2f), (-1, 0), (-0.2f, -0.2f));

            var result = IsConvex();

            Assert.False(result);
        }

        [Fact]
        public void GivenArrowShape_WhenCheckingConvexity_ThenReturnsFalse()
        {
            GivenVertices((2, 0), (0, 1), (0.5f, 0), (0, -1));

            var result = IsConvex();

            Assert.False(result);
        }

        [Fact]
        public void GivenTwoVertices_WhenCheckingConvexity_ThenReturnsFalse()
        {
            GivenVertices((0, 0), (1, 1));

            var result = IsConvex();

            Assert.False(result);
        }

        [Fact]
        public void GivenEmptyList_WhenCheckingConvexity_ThenReturnsFalse()
        {
            TestVertices = new List<Vector2>();

            var result = IsConvex();

            Assert.False(result);
        }

        [Fact]
        public void GivenNullList_WhenCheckingConvexity_ThenReturnsFalse()
        {
            TestVertices = null;

            var result = IsConvex();

            Assert.False(result);
        }

        [Fact]
        public void GivenCollinearPoints_WhenCheckingConvexity_ThenReturnsTrue()
        {
            GivenVertices((0, 0), (0.5f, 0), (1, 0), (1, 1), (0, 1));

            var result = IsConvex();

            Assert.True(result);
        }

        [Fact]
        public void GivenCounterClockwiseSquare_WhenCheckingConvexity_ThenReturnsTrue()
        {
            GivenVertices((0, 0), (0, 1), (1, 1), (1, 0));

            var result = IsConvex();

            Assert.True(result);
        }

        [Fact]
        public void GivenClockwiseSquare_WhenCheckingConvexity_ThenReturnsTrue()
        {
            GivenVertices((0, 0), (1, 0), (1, 1), (0, 1));

            var result = IsConvex();

            Assert.True(result);
        }
    }
}
