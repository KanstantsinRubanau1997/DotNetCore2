
using DotNetCore2;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

var runningType = StartType.BackgroundOnly;

switch(runningType)
{
    case StartType.BackgroundOnly:
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<BackgroundHostedService>();
                })
                .Build();

            await host.RunAsync();
        }
        break;
    case StartType.StandartOnly:
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<CommonHostedService>();
                })
                .Build();

            await host.RunAsync();
        }
        break;
    case StartType.BackgroundAndStandart:
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<CommonHostedService>();
                    services.AddHostedService<BackgroundHostedService>();
                })
                .Build();

            await host.RunAsync();
        }
        break;
    case StartType.ServicesWithLongStartedOne:
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<LongRunningHostedService>();
                    services.AddHostedService<BackgroundHostedService>();
                    services.AddHostedService<CommonHostedService>();
                })
                .Build();

            await host.RunAsync();
        }
        break;
    case StartType.ASP:
        {
            var host = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<BackgroundHostedService>();
                    services.AddHostedService<CommonHostedService>();
                })
                .Build();

            await host.RunAsync();
        }
        break;
}

enum StartType
{
    BackgroundOnly,
    StandartOnly,
    BackgroundAndStandart,
    ServicesWithLongStartedOne, 
    ASP,
}