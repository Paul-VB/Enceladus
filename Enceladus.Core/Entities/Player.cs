using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Physics.Motion.MotionControllers;
using Enceladus.Core.Rendering;
using Enceladus.Core.Entities.Weapons;
using System.Numerics;
using Enceladus.Core.Entities.Weapons.WeaponControllers;

namespace Enceladus.Core.Entities
{
    public class Player : MovableEntity, ICollidable, ISpriteRendered, IArmed
    {
        public List<int> IffCodes { get; set; } = new() { 1 }; // Player team
        public override IHitbox Hitbox { get; set; }
        public override MotionControllerType MotionControllerType { get; set; } = MotionControllerType.PlayerInput;
        public SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.PlayerSubRight;
        public SpriteModifiers SpriteModifiers { get; set; } = new();
        public List<WeaponMount> WeaponMounts { get; set; } = new()
        {
            new WeaponMount { Offset = new Vector2(-1f, 0f), ControllerType = WeaponControllerType.Mouse },
            new WeaponMount { Offset = new Vector2(0.875f, 0f), ControllerType = WeaponControllerType.Mouse }
        };
        
        public static readonly Vector2[] PixelVertices =
        [
            new Vector2(111, 0),
            new Vector2(111, 9),
            new Vector2(124, 20),
            new Vector2(127, 31),
            new Vector2(124, 43),
            new Vector2(111, 54),
            new Vector2(111, 63),
            new Vector2(52, 63),
            new Vector2(12, 56),
            new Vector2(12, 8),
            new Vector2(52, 0)
        ];
    }
}
