using System.Numerics;

namespace Enceladus.Core.Entities.Weapons
{
    public class WeaponMount
    {
        public Vector2 Offset { get; set; }
        public Weapon? EquippedWeapon { get; set; }  // Nullable - mount can be empty
    }
}
