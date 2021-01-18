using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC.AmazonMQ.Consumer.Commands;
using RabbitMQ.Client;
using System;
using System.Reflection;

namespace POC.AmazonMQ.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddMediatR(Assembly.GetExecutingAssembly())
                        .AddTransient<IRequestHandler<LogCommand, Unit>, LogCommandHandler>()
                        .AddHostedService<Worker>() 
                        .AddSingleton(serviceProvider =>
                        {
                            var uri = new Uri("amqps://b-e09fb1d1-bb30-4531-a321-55a07b8a7033.mq.us-east-1.amazonaws.com:5671/pricefy-MQ");
                            return new ConnectionFactory
                            {
                                Uri = uri,
                                UserName = "guest",
                                Password = "guest",
                                DispatchConsumersAsync = true
                            };
                        });
                });
    }
}
