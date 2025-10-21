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
        private readonly Dictionary<string, List<Cell>> _cellsBySprite = new();

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
            // Clear and reuse cached dictionary - no allocations!
            foreach (var list in _cellsBySprite.Values)
            {
                list.Clear();
            }

            // Group cells by sprite type for batching
            foreach (var chunk in visibleChunks)
            {
                foreach (var cell in chunk.Cells)
                {
                    if (!_cellsBySprite.ContainsKey(cell.CellType.SpritePath))
                        _cellsBySprite[cell.CellType.SpritePath] = new List<Cell>();

                    _cellsBySprite[cell.CellType.SpritePath].Add(cell);
                }
            }

            // Draw in batches - one sprite load per type, then draw all cells of that type
            // Skip water cells since they're already drawn as background
            foreach (var (spritePath, cells) in _cellsBySprite)
            {
                // Skip water cells
                //todo: if we are skipping water cells.... maybe we just remove the water cell fully? and treat cell id 0 as nothing. similar to how minecraft does it?
                if (spritePath == Sprites.Water)
                    continue;

                var sprite = _spriteService.Load(spritePath);
                var source = new Rectangle(0, 0, sprite.Width, sprite.Height);

                // Reuse destination rectangle for all cells (1x1 size is constant)
                var dest = new Rectangle(0, 0, 1, 1);
                foreach (var cell in cells)
                {
                    dest.X = cell.X;
                    dest.Y = cell.Y;
                    Raylib.DrawTexturePro(sprite, source, dest, Vector2.Zero, 0f, Color.White);
                }
            }
        }
    }
}
