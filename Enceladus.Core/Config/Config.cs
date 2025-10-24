namespace Enceladus.Core.Config
{
    public class Config
    {
        public PlayerConfig Player { get; set; }
        public PhysicsConfig Physics { get; set; }
        public DisplayConfig Display { get; set; }
        public List<CellTypeConfig> Cell { get; set; } = [];
    }

    public class PlayerConfig
    {
        public float Mass { get; set; }
        public float Drag { get; set; }
        public float AngularDrag { get; set; }
        public float MainEngineThrust { get; set; }
        public float ManeuveringThrust { get; set; }
        public float ManeuveringRotationalAuthority { get; set; }
        public float ManeuveringDampingStrength { get; set; }
        public float ManeuveringFinsAuthority { get; set; }
        public float BrakeStrength { get; set; }
        public float MinVelocityForRotation { get; set; }
        public float MinVelocityForMainEngine { get; set; }
        public float MaxAlignmentErrorDegrees { get; set; }
    }

        public class PhysicsConfig
    {
        public float RestitutionCoefficient { get; set; }
    }

    public class CellTypeConfig
    {
        public int Id { get; set; }
        public int MaxHealth { get; set; }
    }

    public class DisplayConfig
    {
        public int DefaultWindowWidth { get; set; }
        public int DefaultWindowHeight { get; set; }
        public int TargetFps { get; set; }
        public float CameraZoom { get; set; }
    }
}
