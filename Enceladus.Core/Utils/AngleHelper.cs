using System.Numerics;

namespace Enceladus.Utils
{
    /// <summary>
    /// Utility methods for angle calculations and conversions.
    /// Based on proven KSP navigation code from 2022.
    /// </summary>
    public static class AngleHelper
    {
        public static float ClampAngle0To360(float angle)
        {
            return ((angle % 360) + 360) % 360;
        }

        /// <summary>
        /// Converts an angle from trigonometric/normal scale [0, 360] to navigation/signed scale [-180, 180].
        /// Between 0 and 180 degrees, both scales are identical.
        /// Between 180 and 360 degrees in trig scale, nav scale wraps around to -180 and goes up to 0.
        /// This is basically how longitude and latitude work in real life.
        /// </summary>
        public static float ConvertToSignedAngle(float angle)
        {
            float result = ClampAngle0To360(angle);
            if (result > 180f) 
                result -= 360f;
            return result;
        }

        /// <summary>
        /// Calculates the shortest signed angle difference from current to target angle.
        /// Returns a value in the range [-180, 180] representing the shortest rotation path.
        /// Positive values indicate clockwise, negative values indicate counter clockwise
        /// </summary>
        public static float ShortestAngleDifference(float current, float target)
        {
            float result = target - current;
            return ConvertToSignedAngle(result);
        }

        public static Vector2 DegToNormalVector(float degrees)
        {
            var radians = DegToRad(degrees);
            return new Vector2(MathF.Cos(radians), MathF.Sin(radians));
        }

        public static float DegToRad(float degrees) => degrees * (MathF.PI / 180f);

        public static float RadToDeg(float radians) => radians * (180f / MathF.PI);
    }
}
