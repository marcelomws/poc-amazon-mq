using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace POC.AmazonMQ.Consumer.Commands
{
    public class LogCommandHandler : IRequestHandler<LogCommand>
    {
        private readonly ILogger<LogCommandHandler> _logger;

        public LogCommandHandler(ILogger<LogCommandHandler> logger) => _logger = logger;

        public Task<Unit> Handle(LogCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Mensagem recebida: {Message}", request.Message);
            return Task.FromResult(Unit.Value);
        }
    }
}
