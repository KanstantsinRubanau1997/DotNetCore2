using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace DotNetCore2
{
    public class CommonHostedService : IHostedService
    {
        private readonly ILogger<CommonHostedService> _logger;
        private readonly SomeWorker _worker;

        private Timer _timer;

        public CommonHostedService(
            ILogger<CommonHostedService> logger,
            SomeWorker someWorker)
        {
            _logger = logger;
            _worker = someWorker;

            _timer = new Timer(3000);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Start();

            _logger.LogInformation("Start action is running...");
            _timer.Elapsed += (_, _) =>
            {
                try
                {
                    _worker.DoSomeWorkAsync(cancellationToken).Wait();
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Start action is stopped");
                }
            };
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(3));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Dispose();

            _logger.LogWarning("Service is stopped");

            return Task.CompletedTask;
        }
    }
}
