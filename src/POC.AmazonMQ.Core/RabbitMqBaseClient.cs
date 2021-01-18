using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace POC.AmazonMQ.Core
{
    public abstract class RabbitMqBaseClient : IDisposable
    {
        protected const string VirtualHost = "CUSTOM_HOST";
        protected readonly string LoggerExchange = $"{VirtualHost}.LoggerExchange";
        protected readonly string LoggerQueue = $"{VirtualHost}.log.message";
        protected const string LoggerQueueAndExchangeRoutingKey = "log.message";

        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqBaseClient> _logger;

        protected RabbitMqBaseClient(ConnectionFactory connectionFactory, ILogger<RabbitMqBaseClient> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            ConnectToRabbitMq();
        }

        private void ConnectToRabbitMq()
        {
            if (_connection == null || !_connection.IsOpen)
                _connection = _connectionFactory.CreateConnection();

            if (Channel == null || !Channel.IsOpen)
            {
                Channel = _connection.CreateModel();
                Channel.ExchangeDeclare(exchange: LoggerExchange, type: "direct", durable: true, autoDelete: false);
                Channel.QueueDeclare(queue: LoggerQueue, durable: false, exclusive: false, autoDelete: false);
                Channel.QueueBind(queue: LoggerQueue, exchange: LoggerExchange, routingKey: LoggerQueueAndExchangeRoutingKey);
            }
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}
