using Enceladus.Core.Services;
using System.Numerics;

namespace Enceladus.Entities
{
    public interface IPlayer
    {
    }
    public class Player : MoveableEntity, IPlayer
    {
        private readonly IInputManager _inputManager;

        public Player(IInputManager inputManager)
        {
            _inputManager = inputManager;
        }
        public override void Draw()
        {

            Raylib_cs.Raylib.DrawCircle((int)Position.X, (int)Position.Y, 20f, Raylib_cs.Color.Yellow);
        }

        public override void Update(float deltaTime)
        {
            var movementInput = _inputManager.GetMovementInput();
            if (movementInput != Vector2.Zero)
            {
                Accelerate(movementInput * 200f * deltaTime);
            }

            base.Update(deltaTime);
        }
    }
}
