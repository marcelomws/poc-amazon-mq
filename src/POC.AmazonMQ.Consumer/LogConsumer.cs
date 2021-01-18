using MediatR;
using Microsoft.Extensions.Logging;
using POC.AmazonMQ.Consumer.Commands;
using POC.AmazonMQ.Core;
using POC.AmazonMQ.Core.Consumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace POC.AmazonMQ.Consumer
{
    public class LogConsumer : ConsumerBase
    {
        private readonly ILogger<LogConsumer> _logConsumerLogger;
        protected override string QueueName => "log.message";

        public LogConsumer(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            ILogger<LogConsumer> logConsumerLogger,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqBaseClient> logger) :
            base(mediator, connectionFactory, consumerLogger, logger)
        {
            _logConsumerLogger = logConsumerLogger;
        }

        public void ConsumeMessages()
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<LogCommand>;

                var mensagem = Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);

                _logConsumerLogger.LogInformation($"Mensagem recebida: {mensagem}");
            }
            catch (Exception ex)
            {
                _logConsumerLogger.LogCritical(ex, "Falha durante o consumo da mensagem");
            }
        }
    }
}
