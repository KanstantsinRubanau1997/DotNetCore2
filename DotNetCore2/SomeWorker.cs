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

        public Task DoSomeWorkAsync()
        {
            _logger.LogInformation("Doing some work...");
            Thread.Sleep(5000);
            _logger.LogInformation("Finishing some work...");

            return Task.CompletedTask;
        }
    }
}
