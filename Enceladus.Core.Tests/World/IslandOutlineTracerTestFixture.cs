using System.Numerics;
using Enceladus.Core.Utils;
using Enceladus.Core.World.Chunks;

namespace Enceladus.Core.Tests.World
{
    public class IslandOutlineTracerTestFixture
    {
        private readonly IIslandOutlineTracer _islandOutlineTracer;

        public IslandOutlineTracerTestFixture()
        {
            _islandOutlineTracer = new IslandOutlineTracer();
        }

        private Island GivenAnIsland(params (int x, int y)[] cells)
        {
            var island = new Island();
            foreach (var cell in cells)
            {
                island.Add(cell);
            }
            return island;
        }

        private List<Vector2> WhenTracingOutline(Island island)
        {
            return _islandOutlineTracer.TraceOutline(island);
        }

        [Fact]
        public void GivenEmptyIsland_WhenTracing_ThenReturnsNoVertices()
        {
            // Given
            var island = GivenAnIsland();

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Empty(vertices);
        }

        [Fact]
        public void GivenSingleCell_WhenTracing_ThenReturns4Vertices()
        {
            // Given
            var island = GivenAnIsland((5, 5));

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(4, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Single cell at (5,5) should have corners at:
            // (5,5), (6,5), (6,6), (5,6) in some order
            Assert.Contains(new Vector2(5, 5), vertices);
            Assert.Contains(new Vector2(6, 5), vertices);
            Assert.Contains(new Vector2(6, 6), vertices);
            Assert.Contains(new Vector2(5, 6), vertices);
        }

        [Fact]
        public void GivenHorizontalLine2x1_WhenTracing_ThenReturns4Vertices()
        {
            // Given - horizontal line:
            // X X
            var island = GivenAnIsland((5, 5), (6, 5));

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(4, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Rectangle from (5,5) to (7,6) should have 4 corners
            Assert.Contains(new Vector2(5, 5), vertices);
            Assert.Contains(new Vector2(7, 5), vertices);
            Assert.Contains(new Vector2(7, 6), vertices);
            Assert.Contains(new Vector2(5, 6), vertices);
        }

        [Fact]
        public void GivenVerticalLine1x2_WhenTracing_ThenReturns4Vertices()
        {
            // Given - vertical line (Y+ up):
            // X
            // X
            var island = GivenAnIsland((5, 5), (5, 6));

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(4, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Rectangle from (5,5) to (6,7) should have 4 corners
            Assert.Contains(new Vector2(5, 5), vertices);
            Assert.Contains(new Vector2(6, 5), vertices);
            Assert.Contains(new Vector2(6, 7), vertices);
            Assert.Contains(new Vector2(5, 7), vertices);
        }

        [Fact]
        public void Given2x2Square_WhenTracing_ThenReturns4Vertices()
        {
            // Given - 2x2 square (Y+ up):
            // X X
            // X X
            var island = GivenAnIsland((5, 5), (6, 5), (5, 6), (6, 6));

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(4, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Square from (5,5) to (7,7) should have 4 corners
            Assert.Contains(new Vector2(5, 5), vertices);
            Assert.Contains(new Vector2(7, 5), vertices);
            Assert.Contains(new Vector2(7, 7), vertices);
            Assert.Contains(new Vector2(5, 7), vertices);
        }

        [Fact]
        public void Given3x3Square_WhenTracing_ThenReturns4Vertices()
        {
            // Given - 3x3 square (Y+ up):
            // X X X
            // X X X
            // X X X
            var island = GivenAnIsland(
                (5, 5), (6, 5), (7, 5),
                (5, 6), (6, 6), (7, 6),
                (5, 7), (6, 7), (7, 7)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(4, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Square from (5,5) to (8,8) should have 4 corners
            Assert.Contains(new Vector2(5, 5), vertices);
            Assert.Contains(new Vector2(8, 5), vertices);
            Assert.Contains(new Vector2(8, 8), vertices);
            Assert.Contains(new Vector2(5, 8), vertices);
        }

        [Fact]
        public void GivenLShape_WhenTracing_ThenReturns6Vertices()
        {
            // Given - L-shape (Y+ up):
            // X
            // X
            // X X X
            var island = GivenAnIsland(
                (0, 0), (1, 0), (2, 0),
                (0, 1),
                (0, 2)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(6, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // L-shape should have 6 corners (concave polygon)
            // Exact vertices depend on tracing direction, but count should be 6
        }

        [Fact]
        public void GivenTShape_WhenTracing_ThenReturns8Vertices()
        {
            // Given - T-shape (Y+ up):
            //   X
            //   X
            // X X X
            var island = GivenAnIsland(
                (0, 0), (1, 0), (2, 0),
                        (1, 1),
                        (1, 2)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(8, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // T-shape should have 8 corners (more complex concave polygon)
        }

        [Fact]
        public void GivenPlusShape_WhenTracing_ThenReturns12Vertices()
        {
            // Given - Plus/cross shape (Y+ up):
            //   X
            // X X X
            //   X
            var island = GivenAnIsland(
                (1, 0),
                (0, 1), (1, 1), (2, 1),
                (1, 2)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(12, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Plus shape should have 12 corners (4 concave indentations)
        }

        [Fact]
        public void GivenStairStepShape_WhenTracing_ThenReturns6Vertices()
        {
            // Given - Stair step (Y+ up):
            //   X X
            // X X
            var island = GivenAnIsland(
                (0, 0), (1, 0),
                        (1, 1), (2, 1)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(8, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));
        }

        [Fact]
        public void GivenZigZagShape_WhenTracing_ThenReturns8Vertices()
        {
            // Given - Zig-zag (Y+ up):
            //   X
            // X X
            // X
            var island = GivenAnIsland(
                (0, 0),
                (0, 1), (1, 1),
                (1, 2)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(8, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));
        }

        [Fact]
        public void GivenDonutShape_WhenTracing_ThenTracesOuterEdgeOnly()
        {
            // Given - Donut (marching squares traces outer perimeter only, not holes) (Y+ up):
            // X X X X
            // X     X
            // X     X
            // X X X X
            var island = GivenAnIsland(
                // Bottom row
                (0, 0), (1, 0), (2, 0), (3, 0),
                // Left and right columns
                (0, 1), (3, 1),
                (0, 2), (3, 2),
                // Top row
                (0, 3), (1, 3), (2, 3), (3, 3)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            // Should trace outer rectangle only (4 vertices)
            // Note: Current implementation doesn't handle holes - may need enhancement
            Assert.Equal(4, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));

            // Outer rectangle corners
            Assert.Contains(new Vector2(0, 0), vertices);
            Assert.Contains(new Vector2(4, 0), vertices);
            Assert.Contains(new Vector2(4, 4), vertices);
            Assert.Contains(new Vector2(0, 4), vertices);
        }

        //fuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuckfuck
        [Fact]
        public void GivenComplexConcaveShape_WhenTracing_ThenReturnsCorrectVertexCount()
        {
            // Given - Complex concave shape:
            //  XX
            //XXXX
            //X X
            //XXX
            // this diagram fixed by paul already
             
            var island = GivenAnIsland(
                (0, 0), (1, 0), (2, 0),
                (0, 1),         (2, 1),
                (0, 2), (1, 2), (2, 2), (3, 2),
                                (2, 3), (3, 3)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            // Complex shape should have multiple turns
            Assert.True(vertices.Count >= 8);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));
        }

        [Fact]
        public void GivenSnakeShape_WhenTracing_ThenTracesPerimeterCorrectly()
        {
            // Given - Thin snake (Y+ up):
            //     X
            //     X
            // X X X
            var island = GivenAnIsland(
                (0, 0), (1, 0), (2, 0),
                (2, 1),
                (2, 2)
            );

            // When
            var vertices = WhenTracingOutline(island);

            // Then
            Assert.Equal(6, vertices.Count);
            Assert.True(GeometryHelper.IsSimplePolygon(vertices));
        }


        [Fact]
        public void LiterallyTheMostImportantTestEver()
        {
            //arrange
            var one = 1;
            var otherOne = 1;

            //act
            var actualAnswer = one + otherOne;

            //assert
            var expectedAnswer = 2;
            Assert.Equal(expectedAnswer, actualAnswer);
        }
    }
}
