using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class IsConvexTestFixture
    {
        [Fact]
        public void GivenSquare_WhenCheckingConvexity_ThenReturnsTrue()
        {
            // Given - Unit square centered at origin
            var vertices = new List<Vector2>
            {
                new Vector2(-0.5f, -0.5f),
                new Vector2(0.5f, -0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(-0.5f, 0.5f)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenTriangle_WhenCheckingConvexity_ThenReturnsTrue()
        {
            // Given
            var vertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0.5f, 1)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenRegularPentagon_WhenCheckingConvexity_ThenReturnsTrue()
        {
            // Given - Regular pentagon
            var vertices = new List<Vector2>
            {
                new Vector2(0f, -1f),
                new Vector2(0.951f, -0.309f),
                new Vector2(0.588f, 0.809f),
                new Vector2(-0.588f, 0.809f),
                new Vector2(-0.951f, -0.309f)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenLShape_WhenCheckingConvexity_ThenReturnsFalse()
        {
            // Given - L-shaped concave polygon
            var vertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(2, 1),
                new Vector2(1, 1),
                new Vector2(1, 2),
                new Vector2(0, 2)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenStarShape_WhenCheckingConvexity_ThenReturnsFalse()
        {
            // Given - Star shape (concave)
            var vertices = new List<Vector2>
            {
                new Vector2(0, -1),
                new Vector2(0.2f, -0.2f),
                new Vector2(1, 0),
                new Vector2(0.2f, 0.2f),
                new Vector2(0, 1),
                new Vector2(-0.2f, 0.2f),
                new Vector2(-1, 0),
                new Vector2(-0.2f, -0.2f)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenArrowShape_WhenCheckingConvexity_ThenReturnsFalse()
        {
            // Given - Arrow pointing right (concave at the back)
            var vertices = new List<Vector2>
            {
                new Vector2(2, 0),      // tip
                new Vector2(0, 1),      // top back
                new Vector2(0.5f, 0),   // indent
                new Vector2(0, -1)      // bottom back
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenTwoVertices_WhenCheckingConvexity_ThenReturnsFalse()
        {
            // Given - Degenerate polygon (line)
            var vertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(1, 1)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenEmptyList_WhenCheckingConvexity_ThenReturnsFalse()
        {
            // Given
            var vertices = new List<Vector2>();

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenNullList_WhenCheckingConvexity_ThenReturnsFalse()
        {
            // When
            var result = GeometryHelper.IsConvex(null);

            // Then
            Assert.False(result);
        }

        [Fact]
        public void GivenCollinearPoints_WhenCheckingConvexity_ThenReturnsTrue()
        {
            // Given - Rectangle with extra collinear points on edges
            var vertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0.5f, 0),  // Collinear with next point
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then - Should skip collinear points and still be convex
            Assert.True(result);
        }

        [Fact]
        public void GivenCounterClockwiseSquare_WhenCheckingConvexity_ThenReturnsTrue()
        {
            // Given - Counter-clockwise square
            var vertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.True(result);
        }

        [Fact]
        public void GivenClockwiseSquare_WhenCheckingConvexity_ThenReturnsTrue()
        {
            // Given - Clockwise square
            var vertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            // When
            var result = GeometryHelper.IsConvex(vertices);

            // Then
            Assert.True(result);
        }
    }
}
