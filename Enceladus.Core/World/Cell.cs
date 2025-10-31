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

        public int X { get; init; } = 0;
        public int Y { get; init; } = 0;
        public required CellType CellType { get; set; }
        public int Health { get; set; }
        public readonly bool CollisionEnabled => CellType.HasCollision;
        public readonly Vector2 Position => new(X, Y);

        public readonly float Rotation => 0; //cells dont rotate

        public IHitbox Hitbox { get; set; }
    }
}
