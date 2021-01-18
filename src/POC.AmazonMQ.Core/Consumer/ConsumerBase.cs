using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace POC.AmazonMQ.Core.Consumer
{
    public abstract class ConsumerBase : RabbitMqBaseClient
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ConsumerBase> _logger;
        protected abstract string QueueName { get; }

        protected ConsumerBase(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMqBaseClient> logger) :
            base(connectionFactory, logger)
        {
            _mediator = mediator;
            _logger = consumerLogger;
        }

        protected virtual async Task OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = JsonConvert.DeserializeObject<T>(body);

                await _mediator.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Falha no recebimento de mensagem.");
            }
            finally
            {
                Channel.BasicAck(@event.DeliveryTag, true);
            }
        }
    }

}
