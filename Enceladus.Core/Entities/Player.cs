using Enceladus.Core.Input;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using Enceladus.Utils;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Entities
{
    public interface IPlayer
    {
    }
    public class Player : MoveableEntity, ICollidableEntity, IPlayer
    {
        private readonly IInputManager _inputManager;
        private readonly ISpriteService _spriteService;

        public override float Mass { get; set; } = 100f;

        public Hitbox Hitbox { get; set; } = new RectHitbox(8f,4f);

        private readonly float _mainEngineThrust = 22500f;
        private readonly float _manuveringEnginesThrust = 2000f;
        private readonly float _manuveringEnginesRotationalAuthority = 400f;
        private readonly float _manuveringEnginesDampingStrength = 500f;
        private readonly float _manuveringFinsAuthority = 4f;
        public Player(IInputManager inputManager, ISpriteService spriteService)
        {
            _inputManager = inputManager;
            _spriteService = spriteService;


            Sprite = _spriteService.Load(Sprites.PlayerSubRight);
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
                var mainEngineEffectiveThrust = GetMainEngineEffectiveThrust();
                var totalThrust = _manuveringEnginesThrust + mainEngineEffectiveThrust;
                Accelerate(movementInput * totalThrust, deltaTime);
            }

            RotateTowardsVelocityVector(deltaTime);
        }

        private void RotateTowardsVelocityVector(float deltaTime)
        {
            if (Velocity.Length() < 0.1f) return;

            // Control surfaces: authority scales with speed (fins/rudders work better when moving)
            float finAuthority = Velocity.Length() * _manuveringFinsAuthority; 

            // Active stabilization (D term of PD controller)
            // Computer uses thrusters to counter unwanted spin
            float activeDamping = -AngularVelocity * _manuveringEnginesDampingStrength; 

            // 5. Apply total torque
            float totalTorque = (MotionAlignmentError * (_manuveringEnginesRotationalAuthority + finAuthority)) + activeDamping;
            ApplyTorque(totalTorque, deltaTime);
        }

        private float VelocityAngle => MathF.Atan2(Velocity.Y, Velocity.X) * (180f / MathF.PI);
        private float MotionAlignmentError => AngleHelper.ShortestAngleDifference(Rotation, VelocityAngle);
        private float GetMainEngineEffectiveThrust()
        {
            if (Velocity.Length() < 0.1f)
                return 0f; //main engine offline at extremely low speeds

            // Calculate alignment factor (1.0 = perfectly aligned, 0.0 = perpendicular)
            float alignmentError = Math.Abs(MotionAlignmentError);
            float maxAlignmentError = 90f; // 90° = perpendicular, no main engine contribution
            float alignmentFactor = 1f - Math.Clamp(alignmentError / maxAlignmentError, 0f, 1f);

            return _mainEngineThrust * alignmentFactor;
        }
    }
}
