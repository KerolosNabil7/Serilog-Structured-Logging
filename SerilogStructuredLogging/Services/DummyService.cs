namespace SerilogStructuredLogging.Services
{
    public class DummyService : IDummyService
    {
        ILogger<DummyService> _logger;
        public DummyService(ILogger<DummyService> logger)
        {
            _logger = logger;
        }
        public void DoSomething()
        {
            _logger.LogInformation("somethimg is done");
            _logger.LogCritical("oops");
            _logger.LogDebug("nothing much");
        }
    }
}
