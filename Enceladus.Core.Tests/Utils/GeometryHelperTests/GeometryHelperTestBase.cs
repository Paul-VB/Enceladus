using System.Numerics;


namespace Enceladus.Core.Tests.Utils.GeometryHelperTests
{
    public class GeometryHelperTestBase
    {
        protected List<Vector2> TestVertices;
        protected void GivenVertices(params (float x, float y)[] vertices) => TestVertices = vertices.Select(v => new Vector2(v.x, v.y)).ToList();
    }
}
