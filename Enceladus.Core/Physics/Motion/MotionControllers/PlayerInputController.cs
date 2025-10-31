using Enceladus.Core.Config;
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
        private readonly IConfigService _configService;

        public PlayerInputController(IInputReader inputReader, IVelocityUpdater velocityUpdater, IConfigService configService)
        {
            _inputReader = inputReader;
            _velocityUpdater = velocityUpdater;
            _configService = configService;
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
            var config = _configService.Config.Player;
            var movementInput = _inputReader.GetMovementInput();
            if (movementInput != Vector2.Zero)
            {
                var mainEngineEffectiveThrust = GetMainEngineEffectiveThrust(player);
                var totalThrust = config.ManeuveringThrust + mainEngineEffectiveThrust;
                _velocityUpdater.ApplyForce(player, movementInput * totalThrust, deltaTime);
            }
        }

        private void HandleBrakeInput(Player player, float deltaTime)
        {
            var config = _configService.Config.Player;
            if (_inputReader.IsKeyDown(KnownKeyboardControls.Brake))
            {
                _velocityUpdater.ApplyForce(player, -player.Velocity * config.ManeuveringThrust * config.BrakeStrength, deltaTime);
            }
        }

        private void RotateTowardsVelocityVector(Player player, float deltaTime)
        {
            var config = _configService.Config.Player;
            if (player.Velocity.Length() < config.MinVelocityForRotation) return;

            // Control surfaces: authority scales with speed (fins/rudders work better when moving)
            float finAuthority = player.Velocity.Length() * config.ManeuveringFinsAuthority;

            // Active stabilization (D term of PD controller)
            // Computer uses thrusters to counter unwanted spin
            float activeDamping = -player.AngularVelocity * config.ManeuveringDampingStrength;

            float motionAlignmentError = GetMotionAlignmentError(player);
            float totalTorque = motionAlignmentError * (config.ManeuveringRotationalAuthority + finAuthority) + activeDamping;
            _velocityUpdater.ApplyTorque(player, totalTorque, deltaTime);
        }

        private float GetMainEngineEffectiveThrust(Player player)
        {
            var config = _configService.Config.Player;
            if (player.Velocity.Length() < config.MinVelocityForMainEngine)
                return 0f; //main engine offline at extremely low speeds

            // Calculate alignment factor (1.0 = perfectly aligned, 0.0 = perpendicular)
            float alignmentError = Math.Abs(GetMotionAlignmentError(player));
            float alignmentFactor = 1f - Math.Clamp(alignmentError / config.MaxAlignmentErrorDegrees, 0f, 1f);

            return config.MainEngineThrust * alignmentFactor;
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
