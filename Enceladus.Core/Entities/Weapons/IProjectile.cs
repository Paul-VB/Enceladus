namespace Enceladus.Core.Entities.Weapons
{
    public interface IProjectile : IIdentifyFriendFoe
    {
        IArmed Owner { get; set; }
        float TimeToLive { get; set; }
        float SpawnTime { get; set; }
    }
}
