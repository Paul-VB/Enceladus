using Enceladus.Core;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    public static void Main()
    {
        //this is where DI happens
        var serviceProvider = ServiceRegistration.ConfigureServices();

        var gameManager = serviceProvider.GetRequiredService<IGameManager>();
        gameManager.Initialize();
    }
}