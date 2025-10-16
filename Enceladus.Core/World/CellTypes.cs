using Enceladus.Core.Rendering;

namespace Enceladus.Core.World
{
    public static class CellTypes
    {
        //todo: if we plan on deleting water cells... we dont need this do we? 
        public static readonly CellType Water = new()
        {
            Id = 0,
            SpritePath = Sprites.Water,
            HasCollision = false,
        };

        public static readonly CellType Ice = new()
        {
            Id = 1,
            SpritePath = Sprites.Ice,
            HasCollision = true,
        };
    }
}
