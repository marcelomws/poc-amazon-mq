using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC.AmazonMQ.Core.Producer;
using POC.AmazonMQ.Producer.IntegrationEvents;
using RabbitMQ.Client;
using System;

namespace POC.AmazonMQ.Producer
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
                    services.AddHostedService<Worker>();

                    services.AddHostedService<Worker>()
                        .AddSingleton<IRabbitMqProducer<LogIntegrationEvent>, LogProducer>()
                        .AddSingleton(serviceProvider =>
                        {
                            var uri = new Uri("amqps://b-e09fb1d1-bb30-4531-a321-55a07b8a7033.mq.us-east-1.amazonaws.com:5671/pricefy-MQ");
                            return new ConnectionFactory
                            {
                                Uri = uri, UserName="guest", Password = "guest"
                            };
                        });
                });
    }
}
