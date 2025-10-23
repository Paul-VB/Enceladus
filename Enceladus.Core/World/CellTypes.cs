using Enceladus.Core.Rendering;

namespace Enceladus.Core.World
{
    public static class CellTypes
    {

        public static readonly CellType Ice = new()
        {
            Id = 1,
            Sprite = SpriteDefinitions.Cells.Ice,
            HasCollision = true,
        };
    }
}
