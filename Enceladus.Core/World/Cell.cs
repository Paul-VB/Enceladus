using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.World
{
    public struct Cell : ICollidable
    {
        public Cell()
        {
        }

        //todo: maybe refactor cell to just have vector position and not separate x,y?
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public required CellType CellType { get; set; }
        public int Health { get; set; }
        public readonly bool HasCollision => CellType.HasCollision;

        public readonly Vector2 Position => new(X, Y);

        public readonly float Rotation => 0; //cells dont rotate

        public IHitbox Hitbox { get; set; } = new CellHitbox();
    }

    public class CellType
    {
        public int Id { get; set; }

        //todo: if we do switch to one big papa sprite texture as suggested in sprite service, this would need to be a pair of vector2s and not a path
        public string SpritePath { get; set; }
        public int MaxHealth { get; set; } = 0;
        public bool HasCollision { get; set; }
    }
}
