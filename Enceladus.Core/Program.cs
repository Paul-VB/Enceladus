using Enceladus.Core;
using Enceladus.Core.Input;
using Enceladus.Core.Physics.Collision;
using Enceladus.Core.Rendering;
using Enceladus.Core.World;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    public static void Main()
    {
        var serviceProvider = ConfigureServices();

        var gameManager = serviceProvider.GetRequiredService<IGameManager>();
        gameManager.Initialize();
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IWindowManager, WindowManager>();
        services.AddSingleton<IEntityRegistry, EntityRegistry>();
        services.AddSingleton<IInputManager, InputManager>();
        services.AddSingleton<ISpriteService, SpriteService>();
        services.AddSingleton<ICameraManager, CameraManager>();
        services.AddSingleton<IGameManager, GameManager>();
        services.AddSingleton<IWorldService, WorldService>();
        services.AddSingleton<IMapGeneratorService, MapGeneratorService>();

        // Collision services
        services.AddSingleton<ICollisionCheckService, CollisionCheckService>();
        services.AddSingleton<IAabbCollisionDetector, AabbCollisionDetector>();
        services.AddSingleton<IAabbCalculator, AabbCalculator>();
        services.AddSingleton<ISatCollisionDetector, SatCollisionDetector>();
        services.AddSingleton<IVertexExtractor, VertexExtractor>();
        services.AddSingleton<IAxesExtractor, AxesExtractor>();

        return services.BuildServiceProvider();
    }
}