using Enceladus.Core.World;
using Enceladus.Entities;
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
        private const float normalZoom = 16f;

        private readonly IWindowManager _windowManager;
        private Entity? _trackedEntity;

        private Camera2D _camera;
        public Camera2D Camera => _camera;

        public CameraManager(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            InitCamera();
        }

        public void InitCamera()
        {
            _camera = new Camera2D()
            {
                Rotation = 0f,
                Zoom = normalZoom,
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

            float minX = _camera.Target.X - screenWidth / 2f;
            float maxX = _camera.Target.X + screenWidth / 2f;
            float minY = _camera.Target.Y - screenHeight / 2f;
            float maxY = _camera.Target.Y + screenHeight / 2f;

            // Convert to chunk coordinates (with padding for safety)
            var (minChunkX, minChunkY) = Utils.ChunkMath.WorldToChunkCoords((int)minX - 1, (int)minY - 1);
            var (maxChunkX, maxChunkY) = Utils.ChunkMath.WorldToChunkCoords((int)maxX + 1, (int)maxY + 1);

            // Collect visible chunks
            var visibleChunks = new List<MapChunk>();

            for (int chunkX = minChunkX; chunkX <= maxChunkX; chunkX++)
            {
                for (int chunkY = minChunkY; chunkY <= maxChunkY; chunkY++)
                {
                    if (map.Chunks.TryGetValue((chunkX, chunkY), out var chunk))
                    {
                        visibleChunks.Add(chunk);
                    }
                }
            }

            return visibleChunks;
        }
    }
}
