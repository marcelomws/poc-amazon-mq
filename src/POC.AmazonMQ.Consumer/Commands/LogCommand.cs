using MediatR;
using System;

namespace POC.AmazonMQ.Consumer.Commands
{
    public class LogCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
