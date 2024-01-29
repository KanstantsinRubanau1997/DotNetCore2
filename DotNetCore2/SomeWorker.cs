using Microsoft.Extensions.Logging;

namespace DotNetCore2
{
    public class SomeWorker
    {
        private readonly ILogger<SomeWorker> _logger;

        public SomeWorker(ILogger<SomeWorker> logger)
        {
            _logger = logger;
        }

        public async Task DoSomeWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Doing some work...");

            await Task.Delay(5000, cancellationToken);

            _logger.LogInformation("Finishing some work...");
        }
    }
}
