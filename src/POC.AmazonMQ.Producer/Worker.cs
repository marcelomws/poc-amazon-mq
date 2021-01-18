using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using POC.AmazonMQ.Core.Producer;
using POC.AmazonMQ.Producer.IntegrationEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POC.AmazonMQ.Producer
{
    public class Worker : BackgroundService
    {
        private readonly IRabbitMqProducer<LogIntegrationEvent> _producer;
        private readonly ILogger<Worker> _logger;

        public Worker(IRabbitMqProducer<LogIntegrationEvent> producer, ILogger<Worker> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var @event = new LogIntegrationEvent
                {
                    Id = Guid.NewGuid(),
                    Message = $"Hello! Message generated at {DateTime.Now.ToString("O")}"
                };

                _producer.Publish(@event);
                await Task.Delay(10, stoppingToken);
            }

            await Task.CompletedTask;
        }
    }
}
