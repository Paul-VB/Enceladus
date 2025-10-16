namespace Enceladus.Core.Config
{
    public class Config
    {
        public PlayerConfig Player { get; set; }
        public PhysicsConfig Physics { get; set; }
        public List<CellTypeConfig> Cell { get; set; } = new();
    }

    public class PlayerConfig
    {
        public float Mass { get; set; }
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
        public float DefaultDrag { get; set; }
        public float DefaultAngularDrag { get; set; }
        public float MinVelocityThreshold { get; set; }
        public float MinAngularVelocityThreshold { get; set; }
        public float DefaultMass { get; set; }
    }

    public class CellTypeConfig
    {
        public int Id { get; set; }
        public int MaxHealth { get; set; }
    }
}
