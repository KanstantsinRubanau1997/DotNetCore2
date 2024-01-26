using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetCore2
{
    public class CommonHostedService : IHostedService
    {
        private readonly ILogger<CommonHostedService> _logger;
        private readonly SomeWorker _worker;

        public CommonHostedService(
            ILogger<CommonHostedService> logger,
            SomeWorker someWorker)
        {
            _logger = logger;
            _worker = someWorker;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(3));

            _logger.LogInformation("Start action is running...");
            try
            {
                while (await timer.WaitForNextTickAsync(cancellationToken))
                {
                    await _worker.DoSomeWorkAsync();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Start action is stopped");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Service is stopped");

            return Task.CompletedTask;
        }

        public static Task StartHostedService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<CommonHostedService>();
                })
                .Build();

            return host.StartAsync();
        }
    }
}
