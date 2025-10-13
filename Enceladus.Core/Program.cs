using Enceladus.Core.Services;
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
        services.AddSingleton<IGameManager, GameManager>();

        return services.BuildServiceProvider();
    }
}