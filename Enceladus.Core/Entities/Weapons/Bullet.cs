using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public class Bullet : MovableEntity, ICollidable, ISpriteRendered, IProjectile
    {
        public IArmed Owner { get; set; }
        public List<int> IffCodes { get; set; } = new();
        public float TimeToLive { get; set; } = 5f; 
        public float SpawnTime { get; set; }
        public override IHitbox Hitbox { get; set; } = new CircleHitbox(.5f);
        public SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.Bullet;
        public SpriteModifiers SpriteModifiers { get; set; } = new();

        public override float Mass { get; set; } = 2f;

        public Bullet()
        {
            //TODO: MAKE BULLET DRAG CONFIGURABLE IN CONFIG FILE
            // Bullets need very low drag 
            Drag = .1f;
        }
    }
}
