using Enceladus.Core;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    public static void Main()
    {
        var serviceProvider = ServiceRegistration.ConfigureServices();

        var gameManager = serviceProvider.GetRequiredService<IGameManager>();
        gameManager.Initialize();
    }
}