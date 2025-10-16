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
            var waterSprite = _spriteService.Load(Sprites.Water);

            // Set texture to repeat/tile mode
            Raylib.SetTextureWrap(waterSprite, TextureWrap.Repeat);

            foreach (var chunk in visibleChunks)
            {
                // Calculate chunk bounds in world coordinates
                var (chunkWorldX, chunkWorldY) = ChunkMath.ChunkToWorldCoords(chunk.X, chunk.Y);

                // Source: repeat the texture ChunkSize times (one chunk = ChunkSize x ChunkSize cells)
                var source = new Rectangle(0, 0, waterSprite.Width * ChunkMath.ChunkSize, waterSprite.Height * ChunkMath.ChunkSize);

                // Destination: ChunkSize x ChunkSize world units
                var dest = new Rectangle(chunkWorldX, chunkWorldY, ChunkMath.ChunkSize, ChunkMath.ChunkSize);

                Raylib.DrawTexturePro(waterSprite, source, dest, Vector2.Zero, 0f, Color.White);
            }
        }

        private void DrawCells(List<MapChunk> visibleChunks)
        {
            // Group cells by sprite type for batching
            var cellsBySprite = new Dictionary<string, List<Cell>>();

            foreach (var chunk in visibleChunks)
            {
                foreach (var cell in chunk.Cells)
                {
                    if (!cellsBySprite.ContainsKey(cell.CellType.SpritePath))
                        cellsBySprite[cell.CellType.SpritePath] = new List<Cell>();

                    cellsBySprite[cell.CellType.SpritePath].Add(cell);
                }
            }

            // Draw in batches - one sprite load per type, then draw all cells of that type
            // Skip water cells since they're already drawn as background
            foreach (var (spritePath, cells) in cellsBySprite)
            {
                // Skip water cells
                if (spritePath == Sprites.Water)
                    continue;

                var sprite = _spriteService.Load(spritePath);
                var source = new Rectangle(0, 0, sprite.Width, sprite.Height);

                foreach (var cell in cells)
                {
                    var dest = new Rectangle(cell.X, cell.Y, 1, 1);
                    Raylib.DrawTexturePro(sprite, source, dest, Vector2.Zero, 0f, Color.White);
                }
            }
        }
    }
}
