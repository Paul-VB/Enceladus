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

        public bool IsRunning { get; private set; }

        public GameManager(IWindowManager windowManager, IEntityRegistry entityRegistry, ISpriteService spriteService, IInputManager inputManager)
        {
            _windowManager = windowManager;
            _entityRegistry = entityRegistry;
            _spriteService = spriteService;
            _inputManager = inputManager;
        }

        public void Initialize()
        {
            _windowManager.CreateWindow();
            SetupEntities();
            Run();
        }

        public void Stop() => IsRunning = false;

        private void SetupEntities()
        {
            var player = new Player(_inputManager, _spriteService) { Position = new Vector2(50, 50) };
            _entityRegistry.Register(player);
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
        }

        private void DrawAll()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            foreach (var entity in _entityRegistry.Entities.Values)
            {
                entity.Draw();
            }
            Raylib.EndDrawing();

        }

        private void Cleanup()
        {
            Raylib.CloseWindow();
        }
    }
}