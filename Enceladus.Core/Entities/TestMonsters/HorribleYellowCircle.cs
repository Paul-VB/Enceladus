using Enceladus.Core.Input;
using Enceladus.Core.Physics.Hitboxes;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Entities.TestMonsters
{
    public class HorribleYellowCircle : MoveableEntity, ICollidableEntity
    {
        private readonly IInputManager _inputManager;
        public Hitbox Hitbox { get; set; }

        private readonly float _thrust = 5000f;

        public HorribleYellowCircle(IInputManager inputManager)
        {
            _inputManager = inputManager;

            // 3x3 unit circle - radius of 1.5
            Hitbox = new CircleHitbox(1.5f);
            Mass = 50f;
        }

        public override void Update(float deltaTime)
        {
            //HandleInput(deltaTime);
            base.Update(deltaTime);
        }

        private void HandleInput(float deltaTime)
        {
            var movementInput = _inputManager.GetArrowKeyMovementInput();
            if (movementInput != Vector2.Zero)
            {
                Accelerate(movementInput * _thrust, deltaTime);
            }
        }

        public override void Draw(Camera2D camera)
        {
            var circleHitbox = (CircleHitbox)Hitbox;

            // Draw filled circle
            Raylib.DrawCircleV(Position, circleHitbox.Radius, Color.Yellow);

            // Draw outline
            Raylib.DrawCircleLinesV(Position, circleHitbox.Radius, Color.Gold);

            // Draw a line to show rotation
            float radians = Rotation * (MathF.PI / 180f);
            var endPoint = new Vector2(
                Position.X + circleHitbox.Radius * MathF.Cos(radians),
                Position.Y + circleHitbox.Radius * MathF.Sin(radians)
            );
            Raylib.DrawLineV(Position, endPoint, Color.Gold);
        }
    }
}
