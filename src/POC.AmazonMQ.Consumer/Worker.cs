using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POC.AmazonMQ.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly LogConsumer _logConsumer;

        public Worker(ILogger<Worker> logger, LogConsumer logConsumer)
        {
            _logger = logger;
            _logConsumer = logConsumer;
        }

        public LogConsumer LogConsumer { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logConsumer.ConsumeMessages();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
