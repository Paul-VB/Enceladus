using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using Enceladus.Core.Utils;
using Enceladus.Core.World;
using Enceladus.Core.World.Chunks;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Rendering
{
    public interface ICameraManager
    {
        void InitCamera();
        Camera2D Camera { get; }
        void TrackEntity(Entity entity);
        void StopTracking();
        void SetTarget(Vector2 position);
        void Update();
        List<MapChunk> GetVisibleChunks(Map map);
    }

    public class CameraManager : ICameraManager
    {
        private readonly IWindowManager _windowManager;
        private readonly IConfigService _configService;
        private Entity? _trackedEntity;

        private Camera2D _camera;
        public Camera2D Camera => _camera;

        public CameraManager(IWindowManager windowManager, IConfigService configService)
        {
            _windowManager = windowManager;
            _configService = configService;
            InitCamera();
        }

        public void InitCamera()
        {
            _camera = new Camera2D()
            {
                Rotation = 0f,
                Zoom = _configService.Config.Display.CameraZoom,
            };
            CenterCamera();
        }

        public void TrackEntity(Entity entity)
        {
            _trackedEntity = entity;
        }

        public void StopTracking() => _trackedEntity = null;

        public void SetTarget(Vector2 position)
        {
            StopTracking();
            _camera.Target = position; 
        }

        public void Update()
        {
            if (_windowManager.IsResized)
            {
                CenterCamera();
            }

            if (_trackedEntity != null)
            {
                _camera.Target = _trackedEntity.Position;
            }
        }

        private void CenterCamera() =>_camera.Offset = new Vector2(_windowManager.Width / 2f, _windowManager.Height / 2f);

        public List<MapChunk> GetVisibleChunks(Map map)
        {
            // Calculate camera's visible rectangle in world space
            float screenWidth = _windowManager.Width / _camera.Zoom;
            float screenHeight = _windowManager.Height / _camera.Zoom;

            // Add 1 padding for safety to avoid pop-in at screen edges
            float minX = (_camera.Target.X - screenWidth / 2f) - 1;
            float maxX = (_camera.Target.X + screenWidth / 2f) + 1;
            float minY = (_camera.Target.Y - screenHeight / 2f) - 1;
            float maxY = (_camera.Target.Y + screenHeight / 2f) + 1;

            var visibleBounds = GeometryHelper.RectangleFromBounds(minX, maxX, minY, maxY);
            return ChunkMath.GetChunksInBounds(map, visibleBounds).ToList();
        }
    }
}
