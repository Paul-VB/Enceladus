using Enceladus.Core.World;
using System.Numerics;
using Raylib_cs;

namespace Enceladus.Core.Services
{
    public interface IWorldService
    {
        Map CurrentMap { get; }

        Vector2 ClampToBounds(Vector2 position);
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

        public Vector2 ClampToBounds(Vector2 position)
        {
            int halfWidth = CurrentMap.Width / 2;
            int halfHeight = CurrentMap.Height / 2;

            return new Vector2(
                Math.Clamp(position.X, -halfWidth, halfWidth),
                Math.Clamp(position.Y, -halfHeight, halfHeight)
            );
        }
    }
}
