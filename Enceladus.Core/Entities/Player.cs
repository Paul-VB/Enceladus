using Enceladus.Core.Config;
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public interface IPlayer
    {
    }
    public class Player : MoveableEntity, ICollidableEntity, IPlayer
    {
        private readonly IInputManager _inputManager;
        private readonly ISpriteService _spriteService;

        public Hitbox Hitbox { get; set; }

        // Pixel coordinates of submarine hull (from paint.net)
        private static readonly Vector2[] _pixelVertices =
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

        public Player(IInputManager inputManager, ISpriteService spriteService, IConfigService configService)
            : base(configService)
        {
            _inputManager = inputManager;
            _spriteService = spriteService;

            Init();
        }

        private void Init()
        {
            Mass = _configService.Config.Player.Mass;
            Sprite = _spriteService.Load(Sprites.PlayerSubRight);
            Hitbox = PolygonHitboxBuilder.BuildFromPixelCoordinates(Sprite.Width, Sprite.Height, _pixelVertices);
        }

        public override void Update(float deltaTime)
        {
            HandleMovementInput(deltaTime);
            base.Update(deltaTime);
        }

        private void HandleMovementInput(float deltaTime)
        {
            var config = _configService.Config.Player;
            var movementInput = _inputManager.GetMovementInput();
            if (movementInput != Vector2.Zero)
            {
                var mainEngineEffectiveThrust = GetMainEngineEffectiveThrust();
                var totalThrust = config.ManeuveringThrust + mainEngineEffectiveThrust;
                Accelerate(movementInput * totalThrust, deltaTime);
            }
            if (_inputManager.IsKeyDown(KnownInputControls.Brake))
            {
                Accelerate(-Velocity * config.ManeuveringThrust * config.BrakeStrength, deltaTime);
            }

            RotateTowardsVelocityVector(deltaTime);
        }

        private void RotateTowardsVelocityVector(float deltaTime)
        {
            var config = _configService.Config.Player;
            if (Velocity.Length() < config.MinVelocityForRotation) return;

            // Control surfaces: authority scales with speed (fins/rudders work better when moving)
            float finAuthority = Velocity.Length() * config.ManeuveringFinsAuthority;

            // Active stabilization (D term of PD controller)
            // Computer uses thrusters to counter unwanted spin
            float activeDamping = -AngularVelocity * config.ManeuveringDampingStrength;

            float totalTorque = MotionAlignmentError * (config.ManeuveringRotationalAuthority + finAuthority) + activeDamping;
            ApplyTorque(totalTorque, deltaTime);
        }

        private float VelocityAngle => AngleHelper.RadToDeg(MathF.Atan2(Velocity.Y, Velocity.X));
        private float MotionAlignmentError => AngleHelper.ShortestAngleDifference(Rotation, VelocityAngle);
        private float GetMainEngineEffectiveThrust()
        {
            var config = _configService.Config.Player;
            if (Velocity.Length() < config.MinVelocityForMainEngine)
                return 0f; //main engine offline at extremely low speeds

            // Calculate alignment factor (1.0 = perfectly aligned, 0.0 = perpendicular)
            float alignmentError = Math.Abs(MotionAlignmentError);
            float alignmentFactor = 1f - Math.Clamp(alignmentError / config.MaxAlignmentErrorDegrees, 0f, 1f);

            return config.MainEngineThrust * alignmentFactor;
        }
    }
}
