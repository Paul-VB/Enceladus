using System.Numerics;

namespace Enceladus.Core.World
{
    //todo: ideas for what can go here. things like when the next orbital trader is gonna come maybe? or should that be in map?
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
