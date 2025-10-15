using System.Numerics;

namespace Enceladus.Core.World
{
    public interface IWorldService
    {
        Map CurrentMap { get; }
    }

    public class WorldService : IWorldService
    {
        private readonly IMapGeneratorService _mapGeneratorService;

        public Map CurrentMap { get; private set; }

        public WorldService(IMapGeneratorService mapGeneratorService)
        {
            _mapGeneratorService = mapGeneratorService;
            CurrentMap = _mapGeneratorService.GenerateTestMap();
        }
    }
}
