using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotNetCore2
{
    public class LongRunningHostedService : IHostedService
    {
        private readonly ILogger<LongRunningHostedService> _logger;
        private readonly SomeWorker _worker;

        public LongRunningHostedService(
            ILogger<LongRunningHostedService> logger,
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
                    await _worker.DoSomeWorkAsync(cancellationToken);
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
    }
}
