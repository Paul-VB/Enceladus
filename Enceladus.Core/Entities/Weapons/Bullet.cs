using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public class Bullet : Projectile, ISpriteRendered
    {
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
