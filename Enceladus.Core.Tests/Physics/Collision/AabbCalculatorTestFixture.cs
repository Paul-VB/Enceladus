using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Tests.Helpers;
using System.Numerics;

namespace Enceladus.Core.Tests.Physics.Collision
{

    public class AabbCalculatorTestBase
    {
        protected readonly IAabbCalculator _aabbCalculator;

        public AabbCalculatorTestBase()
        {
            _aabbCalculator = new AabbCalculator();
        }
    }


    public class AabbCalculatorCircleHitboxTestFixture : AabbCalculatorTestBase
    {
        [Fact]
        public void CalculateAabb_CircleHitbox_ReturnsSquareBoundingBox()
        {
            // Arrange
            var circle = new CircleHitbox(5f);
            var entity = EntityHelpers.CreateTestEntity(new Vector2(10, 20), circle, 0f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - Circle with radius 5 at (10, 20) should have AABB from (5, 15) to (15, 25)
            Assert.Equal(5f, result.X);
            Assert.Equal(15f, result.Y);
            Assert.Equal(10f, result.Width);
            Assert.Equal(10f, result.Height);
        }
    }

    public class AabbCalculatorRectHitboxTestFixture : AabbCalculatorTestBase
    {
        [Fact]
        public void CalculateAabb_RectHitbox_NoRotation_ReturnsSameSize()
        {
            // Arrange
            var rect = new RectHitbox(new Vector2(8, 6));
            var entity = EntityHelpers.CreateTestEntity(new Vector2(10, 20), rect, 0f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - 8x6 rect centered at (10, 20) should have AABB from (6, 17) with width 8, height 6
            Assert.Equal(6f, result.X);
            Assert.Equal(17f, result.Y);
            Assert.Equal(8f, result.Width);
            Assert.Equal(6f, result.Height);
        }

        [Fact]
        public void CalculateAabb_RectHitbox_90DegreeRotation_SwapsDimensions()
        {
            // Arrange
            var rect = new RectHitbox(new Vector2(8, 6));
            var entity = EntityHelpers.CreateTestEntity(new Vector2(10, 20), rect, 90f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - Rotated 90°, dimensions swap: width becomes ~6, height becomes ~8
            Assert.Equal(10f - 3f, result.X, precision: 2);
            Assert.Equal(20f - 4f, result.Y, precision: 2);
            Assert.Equal(6f, result.Width, precision: 2);
            Assert.Equal(8f, result.Height, precision: 2);
        }

        [Fact]
        public void CalculateAabb_RectHitbox_45DegreeRotation_IncreasesSize()
        {
            // Arrange
            var rect = new RectHitbox(new Vector2(10, 10));
            var entity = EntityHelpers.CreateTestEntity(new Vector2(0, 0), rect, 45f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - 45° rotation of 10x10 square increases AABB to ~14.14x14.14
            float expectedSize = 10f * MathF.Sqrt(2);
            Assert.Equal(-expectedSize / 2f, result.X, precision: 2);
            Assert.Equal(-expectedSize / 2f, result.Y, precision: 2);
            Assert.Equal(expectedSize, result.Width, precision: 2);
            Assert.Equal(expectedSize, result.Height, precision: 2);
        }
    }

    public class AabbCalculatorPolygonHitboxTestFixture : AabbCalculatorTestBase
    {
        [Fact]
        public void CalculateAabb_PolygonHitbox_NoRotation_ReturnsMinMaxBounds()
        {
            // Arrange - Triangle with vertices at (0,0), (4,0), (2,3)
            var vertices = new List<Vector2>
                {
                    new Vector2(0, 0),
                    new Vector2(4, 0),
                    new Vector2(2, 3)
                };
            var polygon = new ConvexPolygonHitbox(vertices);
            var entity = EntityHelpers.CreateTestEntity(new Vector2(10, 20), polygon, 0f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - AABB should contain all vertices when translated to position
            Assert.Equal(10f, result.X, precision: 2);
            Assert.Equal(20f, result.Y, precision: 2);
            Assert.Equal(4f, result.Width, precision: 2);
            Assert.Equal(3f, result.Height, precision: 2);
        }

        [Fact]
        public void CalculateAabb_PolygonHitbox_WithRotation_AccountsForRotatedVertices()
        {
            // Arrange - Square 2x2 centered at origin
            var vertices = new List<Vector2>
                {
                    new Vector2(-1, -1),
                    new Vector2(1, -1),
                    new Vector2(1, 1),
                    new Vector2(-1, 1)
                };
            var polygon = new ConvexPolygonHitbox(vertices);
            var entity = EntityHelpers.CreateTestEntity(new Vector2(0, 0), polygon, 45f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - 45° rotation of 2x2 square increases AABB to ~2.83x2.83
            float expectedSize = 2f * MathF.Sqrt(2);
            Assert.Equal(-expectedSize / 2f, result.X, precision: 2);
            Assert.Equal(-expectedSize / 2f, result.Y, precision: 2);
            Assert.Equal(expectedSize, result.Width, precision: 2);
            Assert.Equal(expectedSize, result.Height, precision: 2);
        }
    }

    public class AabbCalculatorConcavePolygonTestFixture : AabbCalculatorTestBase
    {
        [Fact]
        public void CalculateAabb_ConcavePolygonHitbox_NoRotation_ReturnsOuterBounds()
        {
            // Arrange - L-shaped concave polygon (outer vertices form the L)
            var outerVertices = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(4, 0),
                new Vector2(4, 2),
                new Vector2(2, 2),
                new Vector2(2, 4),
                new Vector2(0, 4)
            };

            // Create dummy slices (not used for AABB calculation)
            var slices = new List<ConvexPolygonHitbox>
            {
                new ConvexPolygonHitbox(new List<Vector2> { new Vector2(0, 0), new Vector2(4, 0), new Vector2(2, 2) }),
                new ConvexPolygonHitbox(new List<Vector2> { new Vector2(0, 0), new Vector2(2, 2), new Vector2(0, 4) })
            };

            var concaveHitbox = new ConcavePolygonHitbox
            {
                Vertices = outerVertices,
                ConvexSlices = slices
            };

            var entity = EntityHelpers.CreateTestEntity(new Vector2(10, 20), concaveHitbox, 0f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - AABB should be 4x4 starting at position (10, 20)
            Assert.Equal(10f, result.X, precision: 2);
            Assert.Equal(20f, result.Y, precision: 2);
            Assert.Equal(4f, result.Width, precision: 2);
            Assert.Equal(4f, result.Height, precision: 2);
        }

        [Fact]
        public void CalculateAabb_ConcavePolygonHitbox_WithRotation_AccountsForRotatedOuterVertices()
        {
            // Arrange - Simple L-shaped concave polygon centered at origin
            var outerVertices = new List<Vector2>
            {
                new Vector2(-2, -2),
                new Vector2(2, -2),
                new Vector2(2, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(-2, 2)
            };

            var slices = new List<ConvexPolygonHitbox>
            {
                new ConvexPolygonHitbox(new List<Vector2> { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(0, 0) })
            };

            var concaveHitbox = new ConcavePolygonHitbox
            {
                Vertices = outerVertices,
                ConvexSlices = slices
            };

            var entity = EntityHelpers.CreateTestEntity(new Vector2(0, 0), concaveHitbox, 90f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - After 90° rotation, the L should still fit in a 4x4 box
            Assert.Equal(-2f, result.X, precision: 2);
            Assert.Equal(-2f, result.Y, precision: 2);
            Assert.Equal(4f, result.Width, precision: 2);
            Assert.Equal(4f, result.Height, precision: 2);
        }

        [Fact]
        public void CalculateAabb_ConcavePolygonHitbox_45DegreeRotation_IncreasesAABB()
        {
            // Arrange - L-shaped concave polygon centered at origin
            var outerVertices = new List<Vector2>
            {
                new Vector2(-2, -2),
                new Vector2(2, -2),
                new Vector2(2, 0),
                new Vector2(0, 0),
                new Vector2(0, 2),
                new Vector2(-2, 2)
            };

            var slices = new List<ConvexPolygonHitbox>
            {
                new ConvexPolygonHitbox(new List<Vector2> { new Vector2(-2, -2), new Vector2(2, -2), new Vector2(0, 0) })
            };

            var concaveHitbox = new ConcavePolygonHitbox
            {
                Vertices = outerVertices,
                ConvexSlices = slices
            };

            var entity = EntityHelpers.CreateTestEntity(new Vector2(0, 0), concaveHitbox, 45f);

            // Act
            var result = _aabbCalculator.CalculateAabb(entity);

            // Assert - 45° rotation increases the AABB size
            // The L-shape's diagonal extent when rotated 45° should be larger than 4x4
            float sqrt2 = MathF.Sqrt(2);
            float expectedApproxSize = 4f * sqrt2;

            // AABB should be roughly centered and larger than original 4x4
            Assert.True(result.Width > 4f, "Width should increase after 45° rotation");
            Assert.True(result.Height > 4f, "Height should increase after 45° rotation");
            Assert.True(result.Width < expectedApproxSize + 1f, "Width should be reasonable");
            Assert.True(result.Height < expectedApproxSize + 1f, "Height should be reasonable");
        }
    }
}
