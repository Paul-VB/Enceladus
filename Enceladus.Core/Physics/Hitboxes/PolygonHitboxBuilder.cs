using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{

    public interface IPolygonHitboxBuilder
    {
        PolygonHitbox BuildFromPixelCoordinates(int spriteWidthPixels, int spriteHeightPixels, Vector2[] pixelVertices);
        ConcavePolygonHitbox BuildFromOuterVertices(List<Vector2> vertices);
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
            var worldVertices = new List<Vector2>();

            foreach (var pixelVertex in pixelVertices)
            {
                // Center coordinates around sprite center
                float centeredX = pixelVertex.X - spriteWidthPixels / 2f;
                float centeredY = pixelVertex.Y - spriteHeightPixels / 2f;

                // Convert from pixels to world units
                float worldX = centeredX / PixelsPerWorldUnit;
                float worldY = centeredY / PixelsPerWorldUnit;

                worldVertices.Add(new Vector2(worldX, worldY));
            }

            return new PolygonHitbox(worldVertices);
        }

        //todo: suggestion - add a BuildFromVertcies that assumes the vertecies are already in world scale... but maybe we dont. a polygon hitbox is literaly just a list of world scale vectors. if you have the world scale vectors, you basically have a polygon hitbox already

        //todo: maybe the code for building a list of concavePolygons goes in here (as mentioned in the SAT collision detector todo) and we keep the polygonHitbox class a skinny class with no logic
        public ConcavePolygonHitbox BuildFromOuterVertices(List<Vector2> vertices)
        {
            var slicesVertices = _concavePolygonSlicer.Slice(vertices);

            var slices = slicesVertices.Select(x => new PolygonHitbox(x)).ToList();

            var concavePolygonHitbox = new ConcavePolygonHitbox()
            {
                OuterVertices = vertices,
                ConvexSlices = slices
            };

            return concavePolygonHitbox;
        }
    }
}
