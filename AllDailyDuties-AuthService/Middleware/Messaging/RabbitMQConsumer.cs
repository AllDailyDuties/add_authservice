using AllDailyDuties_AuthService.Middleware.Messaging.Interfaces;
using AllDailyDuties_AuthService.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AllDailyDuties_AuthService.Middleware.Messaging
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private IUserService _userService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RabbitMQConsumer(IUserService userService, IServiceScopeFactory serviceScopeFactory)
        {
            _userService = userService;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public void ConsumeMessage(IModel channel, string queue)
        {
            channel.QueueDeclare(queue, exclusive: false);
            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Token message received: {message}");
                message = message.Replace("\"", "");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var rabbiqMq = scope.ServiceProvider.GetRequiredService<IRabbitMQConsumer>();
                    var service = scope.ServiceProvider.GetRequiredService<IUserService>();
                    service.SendUserModel(Guid.Parse(message));
                }
                
            };
            //read the message
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }
}
