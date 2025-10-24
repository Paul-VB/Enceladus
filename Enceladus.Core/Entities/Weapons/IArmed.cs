using System.Numerics;

namespace Enceladus.Core.Entities.Weapons
{
    public interface IArmed : IIdentifyFriendFoe
    {
        List<WeaponMount> WeaponMounts { get; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
    }
}
