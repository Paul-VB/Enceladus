using Enceladus.Core.MotionControl.PlayerMotion;
using Enceladus.Core.Entities;
using Enceladus.Core.Physics.Hitboxes;
using System.Numerics;

namespace Enceladus.Core.Tests.Helpers
{
    public class MovableTestEntity : MovableEntity
    {
        public MovableTestEntity()
        {
            Guid = Guid.NewGuid();
            Hitbox = new RectHitbox(new(1, 1)); // Default hitbox
        }

        public override IHitbox Hitbox { get; set; }
        public override PlayerMotionControllerType MotionControllerType { get; set; } = PlayerMotionControllerType.None;

    }
}
