using Enceladus.Core.Services;

namespace Enceladus.Core.World
{
    public static class CellTypes
    {
        public static readonly CellType Water = new()
        {
            Id = 0,
            SpritePath = Sprites.Water,
            MaxHealth = 0, //health isnt really applicable i guess?
            HasCollision = false,
        };

        public static readonly CellType Ice = new()
        {
            Id = 1,
            SpritePath = Sprites.Ice,
            MaxHealth = 100,
            HasCollision = true,
        };
    }
}
