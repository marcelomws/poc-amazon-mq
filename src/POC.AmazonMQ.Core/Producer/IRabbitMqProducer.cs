namespace POC.AmazonMQ.Core.Producer
{
    public interface IRabbitMqProducer<in T>
    {
        void Publish(T @event);
    }
}
