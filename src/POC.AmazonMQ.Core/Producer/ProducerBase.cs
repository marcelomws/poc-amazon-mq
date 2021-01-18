using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace POC.AmazonMQ.Core.Producer
{
    public abstract class ProducerBase<T> : RabbitMqBaseClient, IRabbitMqProducer<T>
    {
        private readonly ILogger<ProducerBase<T>> _logger;
        protected abstract string ExchangeName { get; }
        protected abstract string RoutingKeyName { get; }
        protected abstract string AppId { get; }

        protected ProducerBase(
            ConnectionFactory connectionFactory,
            ILogger<RabbitMqBaseClient> logger,
            ILogger<ProducerBase<T>> producerBaseLogger) :
            base(connectionFactory, logger) => _logger = producerBaseLogger;

        public virtual void Publish(T @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                var properties = Channel.CreateBasicProperties();
                properties.AppId = AppId;
                properties.ContentType = "application/json";
                properties.DeliveryMode = 1; // (1 = Persist To Disk / 2 = Persist )
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                Channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeyName, body: body, basicProperties: properties);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Falha ao publicar mensagem");
            }
        }
    }
}
