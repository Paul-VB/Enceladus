using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Input;
using Enceladus.Core.Physics;
using Enceladus.Core.Rendering;
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
        private readonly ITimeService _timeService;
        private readonly IPhysicsService _physicsService;
        private readonly IRenderingService _renderingService;
        private readonly ISpriteService _spriteService;
        private readonly IInputService _inputService;
        private readonly IWeaponService _weaponService;

        private Player _player;

        public bool IsRunning { get; private set; }

        public GameManager(IConfigService configService, IWindowManager windowManager, IEntityFactory entityFactory,
            ICameraManager cameraManager, ITimeService timeService, IPhysicsService physicsService, IRenderingService renderingService,
            ISpriteService spriteService, IInputService inputService, IWeaponService weaponService)
        {
            _configService = configService;
            _windowManager = windowManager;
            _entityFactory = entityFactory;
            _cameraManager = cameraManager;
            _timeService = timeService;
            _physicsService = physicsService;
            _renderingService = renderingService;
            _spriteService = spriteService;
            _inputService = inputService;
            _weaponService = weaponService;
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
                _timeService.Update(deltaTime);
                _inputService.Update(deltaTime);
                _physicsService.Update(deltaTime);
                _weaponService.Update(deltaTime);
                _renderingService.Render();

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