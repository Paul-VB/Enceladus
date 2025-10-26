using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class DoLineSegmentsIntersectTestFixture
    {
        private Vector2 _seg1Start;
        private Vector2 _seg1End;
        private Vector2 _seg2Start;
        private Vector2 _seg2End;

        private void GivenSegment1((float x, float y) a, (float x, float y) b)
        {
            _seg1Start = new Vector2(a.x, a.y);
            _seg1End = new Vector2(b.x, b.y);
        }

        private void GivenSegment2((float x, float y) a, (float x, float y) b)
        {
            _seg2Start = new Vector2(a.x, a.y);
            _seg2End = new Vector2(b.x, b.y);
        }

        private bool DoSegmentsIntersect() => GeometryHelper.DoLineSegmentsIntersect(_seg1Start, _seg1End, _seg2Start, _seg2End);

        [Fact]
        public void GivenCrossingSegments_WhenCheckingIntersection_ThenReturnsTrue()
        {
            GivenSegment1((0, 0), (2, 2));
            GivenSegment2((0, 2), (2, 0));

            var result = DoSegmentsIntersect();

            Assert.True(result);
        }

        [Fact]
        public void GivenParallelSegments_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (2, 0));
            GivenSegment2((0, 1), (2, 1));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenEndpointTouchingSegments_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (1, 0));
            GivenSegment2((1, 0), (2, 0));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenNonIntersectingSegments_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (1, 1));
            GivenSegment2((2, 2), (3, 3));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenTShapeSegments_WhenCheckingIntersection_ThenReturnsTrue()
        {
            GivenSegment1((0, 1), (2, 1));
            GivenSegment2((1, 0), (1, 2));

            var result = DoSegmentsIntersect();

            Assert.True(result);
        }

        [Fact]
        public void GivenSegmentsIntersectingAtMidpoint_WhenCheckingIntersection_ThenReturnsTrue()
        {
            GivenSegment1((0, 0), (4, 4));
            GivenSegment2((0, 4), (4, 0));

            var result = DoSegmentsIntersect();

            Assert.True(result);
        }

        [Fact]
        public void GivenCollinearNonOverlappingSegments_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (1, 0));
            GivenSegment2((2, 0), (3, 0));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenSegmentsShareStartPoint_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (1, 1));
            GivenSegment2((0, 0), (1, -1));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenVerticalAndHorizontalIntersecting_WhenCheckingIntersection_ThenReturnsTrue()
        {
            GivenSegment1((1, 0), (1, 2));
            GivenSegment2((0, 1), (2, 1));

            var result = DoSegmentsIntersect();

            Assert.True(result);
        }

        [Fact]
        public void GivenVerticalSegmentsParallel_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (0, 2));
            GivenSegment2((1, 0), (1, 2));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenSegmentsAlmostIntersecting_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (0.99f, 1));
            GivenSegment2((1, 0), (1, 1));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenSegmentsIntersectNearEndpoint_WhenCheckingIntersection_ThenReturnsTrue()
        {
            GivenSegment1((0, 0), (2, 2));
            GivenSegment2((0.5f, 2), (2, 0.5f));

            var result = DoSegmentsIntersect();

            Assert.True(result);
        }

        [Fact]
        public void GivenXShapeSegments_WhenCheckingIntersection_ThenReturnsTrue()
        {
            GivenSegment1((-1, -1), (1, 1));
            GivenSegment2((-1, 1), (1, -1));

            var result = DoSegmentsIntersect();

            Assert.True(result);
        }

        [Fact]
        public void GivenOneSegmentInsideOtherBoundingBox_WhenCheckingIntersection_ThenReturnsFalse()
        {
            GivenSegment1((0, 0), (4, 0));
            GivenSegment2((1, 1), (2, 2));

            var result = DoSegmentsIntersect();

            Assert.False(result);
        }

        [Fact]
        public void GivenPerpendicular_ButNotOverlappingLines_ThenReturnFalse()
        {
            GivenSegment1((2, -1), (2, 0.95f));
            GivenSegment2((1, 1), (3, 1));
            var result = DoSegmentsIntersect();
            Assert.False(result);
        }
    }
}
