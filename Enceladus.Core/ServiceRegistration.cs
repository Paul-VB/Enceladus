using Enceladus.Core.Config;
using Enceladus.Core.Entities;
using Enceladus.Core.Entities.Weapons;
using Enceladus.Core.Entities.Weapons.WeaponControllers;
using Enceladus.Core.Input;
using Enceladus.Core.Physics;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Physics.Collision.Detection;
using Enceladus.Core.Physics.Hitboxes.Helpers;
using Enceladus.Core.Rendering;
using Enceladus.Core.World;
using Enceladus.Core.World.Chunks;
using Microsoft.Extensions.DependencyInjection;

namespace Enceladus.Core
{
    public static class ServiceRegistration
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Config service
            services.AddSingleton<IConfigService, ConfigService>();

            // Time service
            services.AddSingleton<ITimeService, TimeService>();

            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IEntityRegistry, EntityRegistry>();
            services.AddSingleton<IControllableRegistry, ControllableRegistry>();
            services.AddSingleton<IInputReader, InputReader>();
            services.AddSingleton<IInputService, InputService>();
            services.AddSingleton<ISpriteService, SpriteService>();
            services.AddSingleton<ICameraManager, CameraManager>();
            services.AddSingleton<IGameManager, GameManager>();
            services.AddSingleton<IWorldService, WorldService>();
            services.AddSingleton<IMapGenerator, MapGenerator>();
            services.AddSingleton<ICellFactory, CellFactory>();
            services.AddSingleton<IChunkHitboxGenerator, ChunkHitboxGenerator>();

            // Entity services
            services.AddSingleton<IPlayerFactory, PlayerFactory>();
            services.AddSingleton<IEntityFactory, EntityFactory>();
            services.AddSingleton<IPolygonHitboxBuilder, PolygonHitboxBuilder>();
            services.AddSingleton<IConcavePolygonSlicer, EarClippingTriangulationSlicer>();

            // Weapon services
            services.AddSingleton<IProjectileFactory, ProjectileFactory>();
            services.AddSingleton<IMouseWeaponController, MouseWeaponController>();
            services.AddSingleton<IWeaponControlService, WeaponControlService>();
            services.AddSingleton<IWeaponService, WeaponService>();

            // Rendering services
            services.AddSingleton<IRenderingService, RenderingService>();
            services.AddSingleton<IMapRenderer, MapRenderer>();
            services.AddSingleton<IEntityRenderer, EntityRenderer>();

            // Physics services (including collision detection/resolution)
            services.AddSingleton<IPhysicsService, PhysicsService>();
            services.AddSingleton<ICollisionService, CollisionService>();
            services.AddSingleton<ICollisionChecker, CollisionChecker>();
            services.AddSingleton<ICollisionResolver, CollisionResolver>();
            services.AddSingleton<IImpactHandlerService, ImpactHandlerService>();
            services.AddSingleton<IAabbCollisionDetector, AabbCollisionDetector>();
            services.AddSingleton<IAabbCalculator, AabbCalculator>();
            services.AddSingleton<ISatCollisionDetector, SatCollisionDetector>();
            services.AddSingleton<ICircleCollisionDetector, CircleCollisionDetector>();
            services.AddSingleton<ICollisionInfoService, CollisionInfoService>();
            services.AddSingleton<IAxesExtractor, AxesExtractor>();

            return services.BuildServiceProvider();
        }
    }
}
