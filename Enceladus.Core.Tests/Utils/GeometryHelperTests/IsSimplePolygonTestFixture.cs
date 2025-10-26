using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class IsSimplePolygonTestFixture : GeometryHelperTestBase
    {
        private bool IsSimplePolygon() => GeometryHelper.IsSimplePolygon(TestVertices);

        [Fact]
        public void GivenUnitSquare_WhenCheckingIfSimple_ThenReturnsTrue()
        {
            GivenVertices((0, 0), (1, 0), (1, 1), (0, 1));

            var result = IsSimplePolygon();

            Assert.True(result);
        }

        [Fact]
        public void GivenTriangle_WhenCheckingIfSimple_ThenReturnsTrue()
        {
            GivenVertices((0, 0), (4, 0), (2, 3));

            var result = IsSimplePolygon();

            Assert.True(result);
        }

        [Fact]
        public void GivenLShapeConcavePolygon_WhenCheckingIfSimple_ThenReturnsTrue()
        {
            GivenVertices((0, 0), (2, 0), (2, 1), (1, 1), (1, 2), (0, 2));

            var result = IsSimplePolygon();

            Assert.True(result);
        }

        [Fact]
        public void GivenRegularHexagon_WhenCheckingIfSimple_ThenReturnsTrue()
        {
            GivenVertices(
                (1f, 0f),
                (0.5f, 0.866f),
                (-0.5f, 0.866f),
                (-1f, 0f),
                (-0.5f, -0.866f),
                (0.5f, -0.866f)
            );

            var result = IsSimplePolygon();

            Assert.True(result);
        }

        [Fact]
        public void GivenSelfIntersectingHexagon_WhenCheckingIfSimple_ThenReturnsFalse()
        {
            // Hexagon with middle two vertices swapped, creating self-intersection
            GivenVertices(
                (1f, 0f),
                (0.5f, 0.866f),
                (-1f, 0f),
                (-0.5f, 0.866f),
                (-0.5f, -0.866f),
                (0.5f, -0.866f)
            );

            var result = IsSimplePolygon();

            Assert.False(result);
        }

        [Fact]
        public void GivenBowtieShape_WhenCheckingIfSimple_ThenReturnsFalse()
        {
            // Classic bowtie: two triangles sharing a vertex, edges cross
            GivenVertices((0, 0), (2, 2), (2, 0), (0, 2));

            var result = IsSimplePolygon();

            Assert.False(result);
        }

        [Fact]
        public void GivenEmptyList_WhenCheckingIfSimple_ThenReturnsFalse()
        {
            TestVertices = new List<Vector2>();

            var result = IsSimplePolygon();

            Assert.False(result);
        }

        [Fact]
        public void GivenNullList_WhenCheckingIfSimple_ThenReturnsFalse()
        {
            TestVertices = null;

            var result = IsSimplePolygon();

            Assert.False(result);
        }

        [Fact]
        public void GivenTwoVertices_WhenCheckingIfSimple_ThenReturnsFalse()
        {
            GivenVertices((0, 0), (1, 1));

            var result = IsSimplePolygon();

            Assert.False(result);
        }
    }
}
