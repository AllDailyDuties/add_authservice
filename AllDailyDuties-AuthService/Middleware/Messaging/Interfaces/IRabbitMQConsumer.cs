using RabbitMQ.Client;

namespace AllDailyDuties_AuthService.Middleware.Messaging.Interfaces
{
    public interface IRabbitMQConsumer
    {
        public void ConsumeMessage(IModel channel, string queue);
        public void ConsumeAuth(IModel channel, string queue);
    }
}
