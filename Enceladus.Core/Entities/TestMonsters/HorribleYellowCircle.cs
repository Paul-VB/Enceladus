using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;
using Enceladus.Core.MotionControl.PlayerMotion;

namespace Enceladus.Core.Entities.TestMonsters
{
    public class HorribleYellowCircle : MovableEntity, IGeometryRendered
    {
        public override IHitbox Hitbox { get; set; }
        public override PlayerMotionControllerType MotionControllerType { get; set; } = PlayerMotionControllerType.None;

        public HorribleYellowCircle()
        {
            // 3x3 unit circle - radius of 1.5
            Hitbox = new CircleHitbox(1.5f);
        }


        public void DrawGeometry(Camera2D camera)
        {
            var circleHitbox = (CircleHitbox)Hitbox;

            // Draw filled circle
            Raylib.DrawCircleV(Position, circleHitbox.Radius, Color.Yellow);

            // Draw outline
            Raylib.DrawCircleLinesV(Position, circleHitbox.Radius, Color.Gold);

            // Draw a line to show rotation
            float radians = AngleHelper.DegToRad(Rotation);
            var endPoint = new Vector2(
                Position.X + circleHitbox.Radius * MathF.Cos(radians),
                Position.Y + circleHitbox.Radius * MathF.Sin(radians)
            );
            Raylib.DrawLineV(Position, endPoint, Color.Gold);
        }
    }
}
