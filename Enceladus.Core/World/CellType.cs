using Enceladus.Core.Rendering;

namespace Enceladus.Core.World
{
    public class CellType
    {
        public required int Id { get; set; }
        public SpriteDefinition Sprite { get; set; } = SpriteDefinitions.Cells.Default;
        public int MaxHealth { get; set; } = 0;
        public bool HasCollision { get; set; }
    }
}
