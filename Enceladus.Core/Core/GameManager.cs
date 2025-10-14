using Enceladus.Core.Input;
using Enceladus.Core.Rendering;
using Enceladus.Core.World;
using Enceladus.Entities;
using Raylib_cs;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Enceladus.Core
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
        private readonly IWorldService _worldService;

        private Player _player;

        public bool IsRunning { get; private set; }

        public GameManager(IWindowManager windowManager, IEntityRegistry entityRegistry, ISpriteService spriteService, IInputManager inputManager,
            ICameraManager cameraManager, IWorldService worldService)
        {
            _windowManager = windowManager;
            _entityRegistry = entityRegistry;
            _spriteService = spriteService;
            _inputManager = inputManager;
            _cameraManager = cameraManager;
            _worldService = worldService;
        }

        public void Initialize()
        {
            _windowManager.CreateWindow();
            _cameraManager.InitCamera();
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

            DrawCells();

            foreach (var entity in _entityRegistry.Entities.Values)
            {
                entity.Draw();
            }

            Raylib.EndMode2D();
            // UI/HUD would be drawn here (outside camera mode)

            Raylib.EndDrawing();
        }

        private void DrawCells()
        {
            var map = _worldService.CurrentMap;
            int halfWidth = map.Width / 2;
            int halfHeight = map.Height / 2;

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var cell = map.Cells[x][y];
                    var sprite = _spriteService.Load(cell.CellType.SpritePath);

                    // Convert from array indices to world coordinates
                    // Array [0,0] maps to world position (-halfWidth, -halfHeight)
                    float worldX = x - halfWidth;
                    float worldY = y - halfHeight;

                    var source = new Rectangle(0, 0, sprite.Width, sprite.Height);
                    var dest = new Rectangle(worldX, worldY, 1, 1); // 1x1 world unit cell

                    Raylib.DrawTexturePro(sprite, source, dest, Vector2.Zero, 0f, Color.White);
                }
            }
        }

        private void Cleanup()
        {
            _spriteService.UnloadAll();
            Raylib.CloseWindow();
        }
    }
}