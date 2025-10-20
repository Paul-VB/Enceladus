using Enceladus.Core.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes.Helpers
{

    public interface IPolygonHitboxBuilder
    {
        PolygonHitbox BuildFromPixelCoordinates(int spriteWidthPixels, int spriteHeightPixels, Vector2[] pixelVertices);
        PolygonHitbox BuildFromVertices(List<Vector2> vertices);
    }
    public class PolygonHitboxBuilder : IPolygonHitboxBuilder
    {
        private const float PixelsPerWorldUnit = 16f;

        private readonly IConcavePolygonSlicer _concavePolygonSlicer;

        public PolygonHitboxBuilder(IConcavePolygonSlicer concavePolygonSlicer)
        {
            _concavePolygonSlicer = concavePolygonSlicer;
        }

        public PolygonHitbox BuildFromPixelCoordinates(int spriteWidthPixels, int spriteHeightPixels, Vector2[] pixelVertices)
        {
            var scaledVectors = new List<Vector2>();

            foreach (var pixelVertex in pixelVertices)
            {
                // Center coordinates around sprite center
                float centeredX = pixelVertex.X - spriteWidthPixels / 2f;
                float centeredY = pixelVertex.Y - spriteHeightPixels / 2f;

                // Convert from pixels to world units
                float worldX = centeredX / PixelsPerWorldUnit;
                float worldY = centeredY / PixelsPerWorldUnit;

                scaledVectors.Add(new Vector2(worldX, worldY));
            }
            return BuildFromVertices(scaledVectors);
        }

        public PolygonHitbox BuildFromVertices(List<Vector2> vertices)
        {
            if (GeometryHelper.IsConvex(vertices))
                return new ConvexPolygonHitbox(vertices);
            else return BuildConcave(vertices);
        }

        private ConcavePolygonHitbox BuildConcave(List<Vector2> vertices)
        {
            var slicesVertices = _concavePolygonSlicer.Slice(vertices);

            var slices = slicesVertices.Select(x => new ConvexPolygonHitbox(x)).ToList();

            var concavePolygonHitbox = new ConcavePolygonHitbox(vertices)
            {
                ConvexSlices = slices
            };

            return concavePolygonHitbox;
        }
    }
}
