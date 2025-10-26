using System.Numerics;

namespace Enceladus.Core.Physics.Hitboxes.Helpers.ConcavePolygonSlicers
{
    /// <summary>
    /// Decomposes concave polygons into a list of convex sub-polygons.
    /// SAT collision detection only works with convex shapes, so concave shapes
    /// must be broken down into convex pieces for collision checks.
    /// </summary>
    public interface IConcavePolygonSlicer
    {
        /// <summary>
        /// Slices a concave polygon into convex sub-polygons.
        /// Returns a list of vertex lists, where each inner list represents one convex polygon.
        /// </summary>
        List<List<Vector2>> Slice(List<Vector2> concavePolygon);
    }
}
