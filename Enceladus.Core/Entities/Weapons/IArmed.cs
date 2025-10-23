using System.Numerics;

namespace Enceladus.Core.Entities.Weapons
{
    public interface IArmed
    {
        List<WeaponMount> WeaponMounts { get; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}
