using Enceladus.Core.World;
using Raylib_cs;
using Color = Raylib_cs.Color;

namespace Enceladus.Core.Rendering
{
    public interface IRenderingService
    {
        void Render();
    }

    public class RenderingService : IRenderingService
    {
        private readonly IWorldService _worldService;
        private readonly IEntityRegistry _entityRegistry;
        private readonly ICameraManager _cameraManager;
        private readonly IMapRenderer _mapRenderer;
        private readonly IEntityRenderer _entityRenderer;

        public RenderingService(IWorldService worldService, IEntityRegistry entityRegistry, ICameraManager cameraManager,
            IMapRenderer mapRenderer, IEntityRenderer entityRenderer)
        {
            _worldService = worldService;
            _entityRegistry = entityRegistry;
            _cameraManager = cameraManager;
            _mapRenderer = mapRenderer;
            _entityRenderer = entityRenderer;
        }

        public void Render()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            // Begin camera mode for world-space rendering
            Raylib.BeginMode2D(_cameraManager.Camera);

            // Render map (water background + cells)
            var map = _worldService.CurrentMap;
            var visibleChunks = _cameraManager.GetVisibleChunks(map);
            _mapRenderer.Draw(map, visibleChunks);

            // Render entities
            _entityRenderer.Draw(_entityRegistry.Entities.Values, _cameraManager.Camera);

            Raylib.EndMode2D();

            // UI/HUD would be drawn here (outside camera mode)

            Raylib.EndDrawing();
        }
    }
}
