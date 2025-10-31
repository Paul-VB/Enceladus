using Enceladus.Core.Entities.Weapons;
using System.Numerics;

namespace Enceladus.Core.Tests.Entities.Weapons
{
    public class TestArmedEntity : IArmed
    {
        public List<int> IffCodes { get; set; } = new();
        public List<WeaponMount> WeaponMounts { get; set; } = new();
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
    }
}
