using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes
{
    public static class PolygonHitboxBuilder
    {
        private const float PixelsPerWorldUnit = 16f;

        public static PolygonHitbox BuildFromPixelCoordinates(int spriteWidthPixels, int spriteHeightPixels, Vector2[] pixelVertices)
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
    }
}
