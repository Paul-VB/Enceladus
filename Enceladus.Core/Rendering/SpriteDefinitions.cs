using Raylib_cs;

namespace Enceladus.Core.Rendering
{
    public class SpriteDefinition
    {
        public required string AtlasFilePath { get; init; }
        public required Rectangle SourceRegion { get; init; }  // Where in the atlas (x, y, width, height)
    }

    public static class SpriteDefinitions
    {

        public const string EntityAtlasFilePath = "assets/sprites/entityTexture.png";
        public const string WaterBackgroundFilePath = "assets/sprites/waterBackground.png";
        public const string CellAtlasFilePath = "assets/sprites/cellTexture.png";

        public static class Entities
        {
            // Default missing texture (magenta checkerboard)
            public static readonly SpriteDefinition DefaultEntity = new()
            {
                AtlasFilePath = EntityAtlasFilePath,
                SourceRegion = new Rectangle(0, 0, 64, 64)
            };

            // Entity sprites
            public static readonly SpriteDefinition PlayerSubRight = new()
            {
                AtlasFilePath = EntityAtlasFilePath,
                SourceRegion = new Rectangle(64, 0, 128, 64)
            };

            public static readonly SpriteDefinition PlayerSubLeft = new()
            {
                AtlasFilePath = EntityAtlasFilePath,
                SourceRegion = new Rectangle(192, 0, 128, 64)
            };

            // Weapon sprites
            public static readonly SpriteDefinition TestGun = new()
            {
                AtlasFilePath = EntityAtlasFilePath,
                SourceRegion = new Rectangle(320, 0, 32, 32)
            };

            // Projectile sprites
            // TODO: Create proper bullet sprite! This is using a chunk of the default checkerboard for testing
            public static readonly SpriteDefinition Bullet = new()
            {
                AtlasFilePath = EntityAtlasFilePath,
                SourceRegion = new Rectangle(12, 12, 8, 8) // 8x8 chunk from default texture
            };
        }

        public static class Cells
        {
            public static readonly SpriteDefinition Default = new()
            {
                AtlasFilePath = CellAtlasFilePath,
                SourceRegion = new Rectangle(0, 0, 16, 16)
            };

            public static readonly SpriteDefinition Ice = new()
            {
                AtlasFilePath = CellAtlasFilePath,
                SourceRegion = new Rectangle(16, 0, 16, 16)
            };
        }




    }
}
