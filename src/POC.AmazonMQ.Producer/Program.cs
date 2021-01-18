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
                            var uri = new Uri("amqps://b-57570d4d-8dc1-4f80-8b7f-50aab9b31aab.mq.us-east-1.amazonaws.com:5671/pricefy-mq");
                            return new ConnectionFactory
                            {
                                Uri = uri, UserName="pricefy", Password = "pricefy@2021"
                            };
                        });
                });
    }
}
