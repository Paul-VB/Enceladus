using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Tests.Helpers;
using Enceladus.Core.World;
using System.Numerics;

namespace Enceladus.Core.Tests.Physics.Collision
{
    public class VertexExtractorTestFixture
    {
        private readonly IVertexExtractor _vertexExtractor;

        public VertexExtractorTestFixture()
        {
            _vertexExtractor = new VertexExtractor();
        }

        [Fact]
        public void ExtractWorldVertices_Cell_ReturnsGridAlignedVertices()
        {
            // Arrange - Cell at grid position (5, 10)
            var cell = new Cell
            {
                X = 5,
                Y = 10,
                CellType = CellTypes.Ice
            };

            // Act
            var vertices = _vertexExtractor.ExtractWorldVertices(cell);

            // Assert - Cell should occupy (5, 10) to (6, 11), NOT centered
            Assert.Equal(4, vertices.Count);
            Assert.Contains(new Vector2(5, 10), vertices);   // top-left
            Assert.Contains(new Vector2(6, 10), vertices);   // top-right
            Assert.Contains(new Vector2(6, 11), vertices);   // bottom-right
            Assert.Contains(new Vector2(5, 11), vertices);   // bottom-left
        }

        [Fact]
        public void ExtractWorldVertices_Cell_NotCentered()
        {
            // Arrange - Cell at origin
            var cell = new Cell
            {
                X = 0,
                Y = 0,
                CellType = CellTypes.Ice
            };

            // Act
            var vertices = _vertexExtractor.ExtractWorldVertices(cell);

            // Assert - Should be (0,0) to (1,1), NOT (-0.5,-0.5) to (0.5,0.5)
            Assert.DoesNotContain(vertices, v => v.X < 0 || v.Y < 0);
            Assert.Contains(new Vector2(0, 0), vertices);
            Assert.Contains(new Vector2(1, 1), vertices);
        }

        [Fact]
        public void ExtractWorldVertices_EntityRectHitbox_CenteredAtPosition()
        {
            // Arrange - Entity with 2x2 rect hitbox at position (10, 20)
            var entity = EntityHelpers.CreateTestEntity(
                new Vector2(10, 20),
                new RectHitbox(2, 2),
                0f
            );

            // Act
            var vertices = _vertexExtractor.ExtractWorldVertices(entity);

            // Assert - 2x2 rect centered at (10, 20) should span (9, 19) to (11, 21)
            Assert.Equal(4, vertices.Count);
            Assert.Contains(new Vector2(9, 19), vertices);   // top-left
            Assert.Contains(new Vector2(11, 19), vertices);  // top-right
            Assert.Contains(new Vector2(11, 21), vertices);  // bottom-right
            Assert.Contains(new Vector2(9, 21), vertices);   // bottom-left
        }

        [Fact]
        public void ExtractWorldVertices_EntityRectHitbox_WithRotation()
        {
            // Arrange - 2x2 square rotated 90 degrees at origin
            var entity = EntityHelpers.CreateTestEntity(
                new Vector2(0, 0),
                new RectHitbox(2, 2),
                90f
            );

            // Act
            var vertices = _vertexExtractor.ExtractWorldVertices(entity);

            // Assert - Should still be centered at origin after rotation
            Assert.Equal(4, vertices.Count);

            // After 90° rotation, vertices should be at approximately:
            // (-1, -1) -> (1, -1)
            // (1, -1) -> (1, 1)
            // (1, 1) -> (-1, 1)
            // (-1, 1) -> (-1, -1)
            // So same positions, just rotated order
            foreach (var vertex in vertices)
            {
                // Each vertex should be at distance sqrt(2) from origin
                float distance = vertex.Length();
                Assert.Equal(MathF.Sqrt(2), distance, precision: 2);
            }
        }

        [Fact]
        public void ExtractWorldVertices_EntityPolygonHitbox_TransformedCorrectly()
        {
            // Arrange - Triangle hitbox (local coords)
            var vertices = new List<Vector2>
            {
                new Vector2(0, -1),  // top
                new Vector2(1, 1),   // bottom-right
                new Vector2(-1, 1)   // bottom-left
            };
            var polygon = new PolygonHitbox(vertices);
            var entity = EntityHelpers.CreateTestEntity(
                new Vector2(10, 20),
                polygon,
                0f
            );

            // Act
            var worldVertices = _vertexExtractor.ExtractWorldVertices(entity);

            // Assert - Local vertices translated by position
            Assert.Equal(3, worldVertices.Count);
            Assert.Contains(new Vector2(10, 19), worldVertices);   // (0, -1) + (10, 20)
            Assert.Contains(new Vector2(11, 21), worldVertices);   // (1, 1) + (10, 20)
            Assert.Contains(new Vector2(9, 21), worldVertices);    // (-1, 1) + (10, 20)
        }
    }
}
