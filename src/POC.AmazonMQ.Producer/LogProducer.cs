using Microsoft.Extensions.Logging;
using POC.AmazonMQ.Core;
using POC.AmazonMQ.Core.Producer;
using POC.AmazonMQ.Producer.IntegrationEvents;
using RabbitMQ.Client;

namespace POC.AmazonMQ.Producer
{
    public class LogProducer : ProducerBase<LogIntegrationEvent>
    {
        public LogProducer(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqBaseClient> logger,
            ILogger<ProducerBase<LogIntegrationEvent>> producerBaseLogger) :
            base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "CUSTOM_HOST.LoggerExchange";
        protected override string RoutingKeyName => "log.message";
        protected override string AppId => "LogProducer";
    }
}
