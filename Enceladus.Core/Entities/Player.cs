
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Hitboxes;
using Enceladus.Core.Rendering;
using Enceladus.Core.Entities.Weapons;
using Enceladus.Utils;
using System.Numerics;

namespace Enceladus.Core.Entities
{
    public class Player : MovableEntity, ICollidable, IControllable, ISpriteRendered, IArmed
    {
        public override IHitbox Hitbox { get; set; }
        public SpriteDefinition CurrentSprite { get; set; } = SpriteDefinitions.Entities.PlayerSubRight;
        public SpriteModifiers SpriteModifiers { get; set; } = new();
        public float MainEngineThrust { get; set; }
        public float ManeuveringThrust { get; set; }
        public float ManeuveringRotationalAuthority { get; set; }
        public float ManeuveringDampingStrength { get; set; }
        public float ManeuveringFinsAuthority { get; set; }
        public float BrakeStrength { get; set; }
        public float MinVelocityForRotation { get; set; }
        public float MinVelocityForMainEngine { get; set; }
        public float MaxAlignmentErrorDegrees { get; set; }
        public List<WeaponMount> WeaponMounts { get; set; } = new()
        {
            new WeaponMount { Offset = new Vector2(-1f, 0f) } 
        };

        private bool _isFacingRight = true;

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

        public void HandleInputs(float deltaTime, IInputReader inputReader)
        {
            var movementInput = inputReader.GetMovementInput();
            if (movementInput != Vector2.Zero)
            {
                var mainEngineEffectiveThrust = GetMainEngineEffectiveThrust();
                var totalThrust = ManeuveringThrust + mainEngineEffectiveThrust;
                Accelerate(movementInput * totalThrust, deltaTime);
            }
            if (inputReader.IsKeyDown(KnownInputControls.Brake))
            {
                Accelerate(-Velocity * ManeuveringThrust * BrakeStrength, deltaTime);
            }

            RotateTowardsVelocityVector(deltaTime);
            UpdateSpriteOrientation();
        }

        private void RotateTowardsVelocityVector(float deltaTime)
        {
            if (Velocity.Length() < MinVelocityForRotation) return;

            // Control surfaces: authority scales with speed (fins/rudders work better when moving)
            float finAuthority = Velocity.Length() * ManeuveringFinsAuthority;

            // Active stabilization (D term of PD controller)
            // Computer uses thrusters to counter unwanted spin
            float activeDamping = -AngularVelocity * ManeuveringDampingStrength;

            float totalTorque = MotionAlignmentError * (ManeuveringRotationalAuthority + finAuthority) + activeDamping;
            ApplyTorque(totalTorque, deltaTime);
        }

        private float VelocityAngle => AngleHelper.RadToDeg(MathF.Atan2(Velocity.Y, Velocity.X));
        private float MotionAlignmentError => AngleHelper.ShortestAngleDifference(Rotation, VelocityAngle);
        private float GetMainEngineEffectiveThrust()
        {
            if (Velocity.Length() < MinVelocityForMainEngine)
                return 0f; //main engine offline at extremely low speeds

            // Calculate alignment factor (1.0 = perfectly aligned, 0.0 = perpendicular)
            float alignmentError = Math.Abs(MotionAlignmentError);
            float alignmentFactor = 1f - Math.Clamp(alignmentError / MaxAlignmentErrorDegrees, 0f, 1f);

            return MainEngineThrust * alignmentFactor;
        }

        //todo: pull this out somewhere in case we wanna re use it on other entities?
        private void UpdateSpriteOrientation()
        {
            if (_isFacingRight)
            {
                if (Rotation > 100f && Rotation < 260f)//todo: magic numbers
                {
                    _isFacingRight = false;
                    CurrentSprite = SpriteDefinitions.Entities.PlayerSubLeft;
                }
            }
            else
            {
                if (Rotation < 80f || Rotation > 280f)//todo: magic numbers
                {
                    _isFacingRight = true;
                    CurrentSprite = SpriteDefinitions.Entities.PlayerSubRight;
                }
            }
        }
    }
}
