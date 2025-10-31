using Enceladus.Core.Entities;
using Enceladus.Core.Input;
using Enceladus.Core.Rendering;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Physics.Motion.MotionControllers
{
    public interface IPlayerInputController
    {
        void Update(Player player, float deltaTime);
    }

    public class PlayerInputController : IPlayerInputController
    {
        private readonly IInputReader _inputReader;
        private readonly IVelocityUpdater _velocityUpdater;

        public PlayerInputController(IInputReader inputReader, IVelocityUpdater velocityUpdater)
        {
            _inputReader = inputReader;
            _velocityUpdater = velocityUpdater;
        }

        public void Update(Player player, float deltaTime)
        {
            HandleMovementInput(player, deltaTime);
            HandleBrakeInput(player, deltaTime);
            RotateTowardsVelocityVector(player, deltaTime);
            UpdateSpriteOrientation(player);
        }

        private void HandleMovementInput(Player player, float deltaTime)
        {
            var movementInput = _inputReader.GetMovementInput();
            if (movementInput != Vector2.Zero)
            {
                var mainEngineEffectiveThrust = GetMainEngineEffectiveThrust(player);
                var totalThrust = player.ManeuveringThrust + mainEngineEffectiveThrust;
                _velocityUpdater.ApplyForce(player, movementInput * totalThrust, deltaTime);
            }
        }

        private void HandleBrakeInput(Player player, float deltaTime)
        {
            if (_inputReader.IsKeyDown(KnownKeyboardControls.Brake))
            {
                _velocityUpdater.ApplyForce(player, -player.Velocity * player.ManeuveringThrust * player.BrakeStrength, deltaTime);
            }
        }

        private void RotateTowardsVelocityVector(Player player, float deltaTime)
        {
            if (player.Velocity.Length() < player.MinVelocityForRotation) return;

            // Control surfaces: authority scales with speed (fins/rudders work better when moving)
            float finAuthority = player.Velocity.Length() * player.ManeuveringFinsAuthority;

            // Active stabilization (D term of PD controller)
            // Computer uses thrusters to counter unwanted spin
            float activeDamping = -player.AngularVelocity * player.ManeuveringDampingStrength;

            float motionAlignmentError = GetMotionAlignmentError(player);
            float totalTorque = motionAlignmentError * (player.ManeuveringRotationalAuthority + finAuthority) + activeDamping;
            _velocityUpdater.ApplyTorque(player, totalTorque, deltaTime);
        }

        private float GetMainEngineEffectiveThrust(Player player)
        {
            if (player.Velocity.Length() < player.MinVelocityForMainEngine)
                return 0f; //main engine offline at extremely low speeds

            // Calculate alignment factor (1.0 = perfectly aligned, 0.0 = perpendicular)
            float alignmentError = Math.Abs(GetMotionAlignmentError(player));
            float alignmentFactor = 1f - Math.Clamp(alignmentError / player.MaxAlignmentErrorDegrees, 0f, 1f);

            return player.MainEngineThrust * alignmentFactor;
        }

        private float GetVelocityAngle(Player player) => AngleHelper.RadToDeg(MathF.Atan2(player.Velocity.Y, player.Velocity.X));

        private float GetMotionAlignmentError(Player player) => AngleHelper.ShortestAngleDifference(player.Rotation, GetVelocityAngle(player));

        private void UpdateSpriteOrientation(Player player)
        {
            // Access private field via public property pattern or make this a separate service
            // For now, we'll handle this inline
            bool isFacingRight = player.CurrentSprite == SpriteDefinitions.Entities.PlayerSubRight;

            if (isFacingRight)
            {
                if (player.Rotation > 100f && player.Rotation < 260f)
                {
                    player.CurrentSprite = SpriteDefinitions.Entities.PlayerSubLeft;
                }
            }
            else
            {
                if (player.Rotation < 80f || player.Rotation > 280f)
                {
                    player.CurrentSprite = SpriteDefinitions.Entities.PlayerSubRight;
                }
            }
        }
    }
}
