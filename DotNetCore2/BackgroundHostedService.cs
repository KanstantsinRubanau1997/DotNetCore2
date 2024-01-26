using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetCore2
{
    public class BackgroundHostedService : BackgroundService
    {
        private readonly ILogger<BackgroundHostedService> _logger;
        private readonly SomeWorker _worker;

        public BackgroundHostedService(
            ILogger<BackgroundHostedService> logger,
            SomeWorker someWorker)
        {
            _logger = logger;
            _worker = someWorker;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
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

        public static Task StartHostedService()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<SomeWorker>();
                    services.AddHostedService<BackgroundHostedService>();
                })
                .Build();

            return host.StartAsync();
        }
    }
}
