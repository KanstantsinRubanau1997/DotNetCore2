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
            var shouldUseToken = true;
            var actualCancelationToken = shouldUseToken
                ? cancellationToken
                : new CancellationTokenSource().Token;
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(3));

            _logger.LogInformation("Start action is running...");
            try
            {
                while (await timer.WaitForNextTickAsync(actualCancelationToken))
                {
                    await _worker.DoSomeWorkAsync(actualCancelationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Start action is stopped");
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Service is stopped");

            cancellationToken.Register(() => _logger.LogCritical("StopAsync token is cancelled"));

            return base.StopAsync(cancellationToken);
        }
    }
}

// https://stackoverflow.com/questions/61094552/why-is-iscancellationrequested-not-set-to-true-on-stopping-a-backgroundservice-i