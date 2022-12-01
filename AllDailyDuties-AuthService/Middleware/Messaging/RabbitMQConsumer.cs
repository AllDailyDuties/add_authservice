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
        public void ConsumeAuth(IModel channel, string queue)
        {
            channel.QueueDeclare(queue, exclusive: false);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                string response = null;
                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    message = message.Replace("\"", "");
                    response = message;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    response = "";
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                        basicProperties: replyProps, body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
        }

        public void ConsumeMessage(IModel channel, string queue)
        {
            channel.QueueDeclare(queue, exclusive: false);
            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var props = eventArgs.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Token message received: {message}");
                message = message.Replace("\"", "");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var rabbiqMq = scope.ServiceProvider.GetRequiredService<IRabbitMQConsumer>();
                    var service = scope.ServiceProvider.GetRequiredService<IUserService>();
                    service.SendUserModel(Guid.Parse(message), replyProps);
                }
                
            };
            //read the message
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }
    }
}
