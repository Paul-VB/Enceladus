namespace Enceladus.Core.Physics.Hitboxes
{
    public class CircleHitbox : Hitbox
    {
        public float Radius { get; set; }

        public CircleHitbox(float radius)
        {
            Radius = radius;
        }
    }
}
