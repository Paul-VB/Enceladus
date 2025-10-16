using Enceladus.Core.Config;
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Rendering;
using Enceladus.Core.World;
using Enceladus.Entities;
using Enceladus.Entities.TestMonsters;
using Raylib_cs;
using System.Numerics;

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
        private readonly IConfigService _configService;
        private readonly IWindowManager _windowManager;
        private readonly IEntityRegistry _entityRegistry;
        private readonly ISpriteService _spriteService;
        private readonly IInputManager _inputManager;
        private readonly ICameraManager _cameraManager;
        private readonly IWorldService _worldService;
        private readonly ICollisionService _collisionService;
        private readonly IRenderingService _renderingService;

        private Player _player;

        public bool IsRunning { get; private set; }

        public GameManager(IConfigService configService, IWindowManager windowManager, IEntityRegistry entityRegistry, ISpriteService spriteService, IInputManager inputManager,
            ICameraManager cameraManager, IWorldService worldService, ICollisionService collisionService, IRenderingService renderingService)
        {
            _configService = configService;
            _windowManager = windowManager;
            _entityRegistry = entityRegistry;
            _spriteService = spriteService;
            _inputManager = inputManager;
            _cameraManager = cameraManager;
            _worldService = worldService;
            _collisionService = collisionService;
            _renderingService = renderingService;
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
            _player = new Player(_inputManager, _spriteService, _configService) { Position = new Vector2(0, 0) };
            _entityRegistry.Register(_player);

            SpawnTestMonsters();

        }

        private void SpawnTestMonsters()
        {
            var triangle = new EvilBlueTriangle(_inputManager, _configService)
            {
                Position = new Vector2(5, 5)
            };
            _entityRegistry.Register(triangle);

            var pentagon = new MenacingRedPentagon(_configService)
            {
                Position = new Vector2(-8, 3),
                Velocity = new Vector2(-1, 2),
                AngularVelocity = -30f
            };
            _entityRegistry.Register(pentagon);

            var circle = new HorribleYellowCircle(_inputManager, _configService)
            {
                Position = new Vector2(0, -10)
            };
            _entityRegistry.Register(circle);
        }

        private void Run()
        {
            IsRunning = true;

            while (IsRunning && !Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                UpdateAll(deltaTime);
                _renderingService.Render();

                _windowManager.SetTitle($"Enceladus - FPS: {_windowManager.GetFps()}");
            }

            Cleanup();
        }

        private void UpdateAll(float deltaTime)
        {
            foreach (var entity in _entityRegistry.Entities.Values)
            {
                entity.Update(deltaTime);
            }

            _collisionService.HandleCollisions(_entityRegistry.Entities.Values, _worldService.CurrentMap);

            _cameraManager.Update();
        }

        private void Cleanup()
        {
            _spriteService.UnloadAll();
            Raylib.CloseWindow();
        }
    }
}