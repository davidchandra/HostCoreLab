namespace HostCoreDocker
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;


    public class WorkerHostedService : IHostedService
    {
        private readonly ILogger _log;
        private IConfiguration _config;
        private MyConfiguration _myconfig = new MyConfiguration();

        public WorkerHostedService(ILogger<WorkerHostedService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
            var sectionExists = _config.GetSection("seqSettings").Exists();
            _config.GetSection("seqSettings").Bind(_myconfig);
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("StartAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("StopAsync");
            return Task.CompletedTask;
        }
    }
}
