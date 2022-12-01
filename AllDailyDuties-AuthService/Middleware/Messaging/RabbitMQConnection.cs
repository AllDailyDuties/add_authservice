using RabbitMQ.Client;

namespace AllDailyDuties_AuthService.Middleware.Messaging
{
    public class RabbitMQConnection
    {
        public IModel Connect()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using var channel = connection.CreateModel();
            return channel;
        }
    }
}
