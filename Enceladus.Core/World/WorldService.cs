using System.Numerics;

namespace Enceladus.Core.World
{
    public interface IWorldService
    {
        Map CurrentMap { get; }
    }

    public class WorldService : IWorldService
    {
        private readonly IMapGenerator _mapGenerator;

        public Map CurrentMap { get; private set; }

        public WorldService(IMapGenerator mapGenerator)
        {
            _mapGenerator = mapGenerator;
            CurrentMap = _mapGenerator.GenerateTestMap();
        }
    }
}
