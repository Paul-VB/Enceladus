using Enceladus.Core.Services;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Entities
{
    public interface IPlayer
    {
    }
    public class Player : MoveableEntity, IPlayer
    {
        private readonly IInputManager _inputManager;
        private readonly ISpriteService _spriteService;

        public override float Mass { get; set; } = 100f;

        private readonly float _mainEngineThrust = 17500f;
        public Player(IInputManager inputManager, ISpriteService spriteService)
        {
            _inputManager = inputManager;
            _spriteService = spriteService;


            Sprite = _spriteService.Load(Sprites.PlayerSubLeft);
        }
        public override void Draw()
        {
            Raylib.DrawTextureV(this.Sprite, Position, Color.White);
        }

        public override void Update(float deltaTime)
        {
            handleMovementInput(deltaTime);
            base.Update(deltaTime);
        }

        private void handleMovementInput(float deltaTime)
        {
            var movementInput = _inputManager.GetMovementInput();
            if (movementInput != Vector2.Zero)
            {
                Accelerate(movementInput * _mainEngineThrust, deltaTime);
            }
        }

        private void RotateTowardsVelocityVector(float deltaTime)
        {
            throw new NotImplementedException();
            //var VelocityVectorAngle = null; //convert 
        }
    }
}
