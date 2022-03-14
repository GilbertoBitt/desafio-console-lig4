using System;
using System.Threading.Tasks;
using ConsoleLig4.Core.Interfaces;
using ConsoleLig4.Core.Models;
using ConsoleLig4.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleLig4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            //change to use ImprovedAiService
            _ = services.AddSingleton<IAIService, ImprovedAiService>();
            _ = services.AddTransient<IGameService, GameService>();
            _ = services.AddSingleton<IInputService, InputService>();
            _ = services.AddSingleton<IPrintService, PrintService>();
            _ = services.AddSingleton<Configuration>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            Configuration configuration = serviceProvider.GetService<Configuration>();
            configuration.BoardSize = 5; // mínimo 5

            IGameService gameService = serviceProvider.GetService<IGameService>();
            await gameService.PlayAsync();
        }
    }
}
