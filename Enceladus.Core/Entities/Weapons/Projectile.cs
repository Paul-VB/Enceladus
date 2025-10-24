namespace Enceladus.Core.Entities.Weapons
{
    public abstract class Projectile : MovableEntity, IIdentifyFriendFoe
    {
        public IArmed Owner { get; set; }
        public List<int> IffCodes { get; set; } = new();
        public float TimeToLive { get; set; } = 5f;
        public float SpawnTime { get; set; }
    }
}
