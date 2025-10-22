using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using Enceladus.Core.Input;
using Enceladus.Core.Physics;
using Enceladus.Core.Rendering;
using Enceladus.Core.World;
using Enceladus.Core.Entities.TestMonsters;
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
        private readonly IEntityFactory _entityFactory;
        private readonly ICameraManager _cameraManager;
        private readonly IPhysicsService _physicsService;
        private readonly IRenderingService _renderingService;
        private readonly ISpriteService _spriteService;
        private readonly IInputService _inputService;

        private Player _player;

        public bool IsRunning { get; private set; }

        public GameManager(IConfigService configService, IWindowManager windowManager, IEntityFactory entityFactory,
            ICameraManager cameraManager, IPhysicsService physicsService, IRenderingService renderingService,
            ISpriteService spriteService, IInputService inputService)
        {
            _configService = configService;
            _windowManager = windowManager;
            _entityFactory = entityFactory;
            _cameraManager = cameraManager;
            _physicsService = physicsService;
            _renderingService = renderingService;
            _spriteService = spriteService;
            _inputService = inputService;
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
            _player = _entityFactory.CreatePlayer(new Vector2(0, 0));
            SpawnTestMonsters();
        }

        private void SpawnTestMonsters()
        {
            _entityFactory.CreateEvilBlueTriangle(new Vector2(5, 5));

            var pentagon = _entityFactory.CreateMenacingRedPentagon(new Vector2(-8, 3));
            pentagon.Velocity = new Vector2(-1, 2);
            pentagon.AngularVelocity = -30f;

            _entityFactory.CreateHorribleYellowCircle(new Vector2(0, -10));

            _entityFactory.CreateAwfulGreenStar(new Vector2(10, 0));
        }

        private void Run()
        {
            IsRunning = true;

            while (IsRunning && !Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                _physicsService.Update(deltaTime);
                _renderingService.Render();
                _inputService.Update(deltaTime);

                _windowManager.SetTitle($"Enceladus - FPS: {_windowManager.GetFps()}");
            }

            Cleanup();
        }

        private void Cleanup()
        {
            _spriteService.UnloadAll();
            Raylib.CloseWindow();
        }
    }
}