using Enceladus.Core.Entities;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Rendering
{
    public interface IEntityRenderer
    {
        void DrawAllEntities(Camera2D camera);
    }

    public class EntityRenderer : IEntityRenderer
    {
        private readonly IEntityRegistry _entityRegistry;
        private readonly ISpriteService _spriteService;
        public EntityRenderer(IEntityRegistry entityRegistry, ISpriteService spriteService)
        {
            _entityRegistry = entityRegistry;
            _spriteService = spriteService;
        }
        public void DrawAllEntities(Camera2D camera)
        {
            DrawSpriteRenderedEntities(camera);
            DrawGeometryRenderedEntities(camera);
        }

        private void DrawSpriteRenderedEntities(Camera2D camera)
        {
            foreach (var entity in _entityRegistry.SpriteRenderedEntities)
            {
                var spriteDef = entity.CurrentSprite;
                var mods = entity.SpriteModifiers;
                var atlas = _spriteService.GetTextureAtlas(spriteDef.AtlasFilePath);

                var size = new Vector2(spriteDef.SourceRegion.Width / camera.Zoom, spriteDef.SourceRegion.Height / camera.Zoom);
                var origin = size / 2f;
                var dest = new Rectangle(entity.Position, size);

                // Apply flip modifiers to source rectangle
                var source = spriteDef.SourceRegion;
                if (mods.FlipHorizontal) source.Width = -source.Width;
                if (mods.FlipVertical) source.Height = -source.Height;

                // Apply tint + alpha
                var tint = mods.Tint;
                tint.A = mods.Alpha;

                // Apply blend mode and draw
                Raylib.BeginBlendMode(mods.BlendMode);
                Raylib.DrawTexturePro(atlas, source, dest, origin, entity.Rotation, tint);
                Raylib.EndBlendMode();
            }
        }

        private void DrawGeometryRenderedEntities(Camera2D camera)
        {
            foreach (var entity in _entityRegistry.GeometryRenderedEntities)
            {
                entity.DrawGeometry(camera);
            }
        }
    }
}
