using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;

namespace Enceladus.Core.Services
{
    public interface ICameraManager
    {
        Camera2D Camera { get; }
        void TrackEntity(Entity entity);
        void StopTracking();
        void SetTarget(Vector2 position);
        void Update();
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

        private void InitCamera()
        {
            _camera = new Camera2D()
            {
                Rotation = 0f,
                Zoom = normalZoom,
                Offset = new Vector2(960, 540)
            };
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
            if (_trackedEntity != null)
            {
                _camera.Target = _trackedEntity.Position;
            }
        }
    }
}
