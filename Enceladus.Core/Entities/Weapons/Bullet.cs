using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;

namespace Enceladus.Core.Entities.Weapons
{
    public class Bullet : Projectile, ISpriteRendered
    {
        public override IHitbox Hitbox { get; set; } = new CircleHitbox(.5f);
        public SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.Bullet;
        public SpriteModifiers SpriteModifiers { get; set; } = new();
        public override float Mass { get; set; } = 2f; //todo: make this configurable per bullet type
        public override float TimeToLive { get; set; } = 5f;
        public new float Drag = .1f;
    }
}
