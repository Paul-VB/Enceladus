using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Enceladus.Core.Services
{
    public interface IGameManager
    {
        void Initialize();
        void Stop();
        bool IsRunning { get; }
    }

    public class GameManager : IGameManager
    {
        private readonly IWindowManager _windowManager;
        private readonly IEntityRegistry _entityRegistry;
        private readonly ISpriteService _spriteService;
        private readonly IInputManager _inputManager;
        private readonly ICameraManager _cameraManager;

        private Player _player;

        public bool IsRunning { get; private set; }

        public GameManager(IWindowManager windowManager, IEntityRegistry entityRegistry, ISpriteService spriteService, IInputManager inputManager,
            ICameraManager cameraManager)
        {
            _windowManager = windowManager;
            _entityRegistry = entityRegistry;
            _spriteService = spriteService;
            _inputManager = inputManager;
            _cameraManager = cameraManager;
        }

        public void Initialize()
        {
            _windowManager.CreateWindow();
            SetupEntities();
            _cameraManager.TrackEntity(_player);
            Run();
        }

        public void Stop() => IsRunning = false;

        private void SetupEntities()
        {
            _player = new Player(_inputManager, _spriteService) { Position = new Vector2(0,0) };
            _entityRegistry.Register(_player);
        }

        private void Run()
        {
            IsRunning = true;

            while (IsRunning && !Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                UpdateAll(deltaTime);
                DrawAll();
            }

            Cleanup();
        }

        private void UpdateAll(float deltaTime)
        {
            foreach (var entity in _entityRegistry.Entities.Values)
            {
                entity.Update(deltaTime);
            }

            _cameraManager.Update();
        }

        private void DrawAll()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // Begin camera mode for world-space rendering
            Raylib.BeginMode2D(_cameraManager.Camera);

            foreach (var entity in _entityRegistry.Entities.Values)
            {
                entity.Draw();
            }

            Raylib.EndMode2D();
            // UI/HUD would be drawn here (outside camera mode)

            Raylib.EndDrawing();
        }

        private void Cleanup()
        {
            _spriteService.UnloadAll();
            Raylib.CloseWindow();
        }
    }
}