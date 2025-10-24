using Enceladus.Core.Config;
using Enceladus.Core.World;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Rendering
{
    public interface IMapRenderer
    {
        void Draw(Map map, List<MapChunk> visibleChunks);
    }

    public class MapRenderer : IMapRenderer
    {
        private readonly ISpriteService _spriteService;

        public MapRenderer(ISpriteService spriteService)
        {
            _spriteService = spriteService;
        }

        public void Draw(Map map, List<MapChunk> visibleChunks)
        {
            DrawTiledWaterBackground(visibleChunks);
            DrawCells(visibleChunks);
        }

        private void DrawTiledWaterBackground(List<MapChunk> visibleChunks)
        {
            var waterSprite = _spriteService.GetTextureAtlas(SpriteDefinitions.WaterBackgroundFilePath);

            // Set texture to repeat/tile mode
            Raylib.SetTextureWrap(waterSprite, TextureWrap.Repeat);

            foreach (var chunk in visibleChunks)
            {
                // Calculate chunk bounds in world coordinates
                var (chunkWorldX, chunkWorldY) = ChunkMath.ChunkToWorldCoords(chunk.X, chunk.Y);

                // Source: repeat the texture ChunkSize times (one chunk = ChunkSize x ChunkSize cells)
                var source = new Rectangle(0, 0, waterSprite.Width * Constants.ChunkSize, waterSprite.Height * Constants.ChunkSize);

                // Destination: ChunkSize x ChunkSize world units
                var dest = new Rectangle(chunkWorldX, chunkWorldY, Constants.ChunkSize, Constants.ChunkSize);

                Raylib.DrawTexturePro(waterSprite, source, dest, Vector2.Zero, 0f, Color.White);
            }
        }

        private void DrawCells(List<MapChunk> visibleChunks)
        {
            var atlas = _spriteService.GetTextureAtlas(SpriteDefinitions.CellAtlasFilePath);
            // Reuse destination rectangle for all cells. we dont need to allocate a new one for each cell, and (1x1 size is constant)
            var dest = new Rectangle(0, 0, 1, 1);

            foreach (var chunk in visibleChunks)
            {
                foreach (var cell in chunk.Cells)
                {
                    var spriteDef = cell.CellType.Sprite;
                    var source = spriteDef.SourceRegion;
                    dest.X = cell.X;
                    dest.Y = cell.Y;
                    Raylib.DrawTexturePro(atlas, source, dest, Vector2.Zero, 0f, Color.White);
                }
            }
        }
    }
}
