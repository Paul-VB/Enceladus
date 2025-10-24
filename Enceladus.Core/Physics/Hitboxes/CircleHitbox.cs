using System.Diagnostics.CodeAnalysis;

namespace Enceladus.Core.Physics.Hitboxes
{
    public class CircleHitbox : IHitbox
    {
        public required float Radius { get; set; }

        public CircleHitbox() { }

        [SetsRequiredMembers]
        public CircleHitbox(float radius)
        {
            Radius = radius;
        }
    }
}
