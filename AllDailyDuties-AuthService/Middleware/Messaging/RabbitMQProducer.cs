using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace AllDailyDuties_AuthService.Middleware.Messaging
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendMessage<T>(T message, string queue, IBasicProperties props)
        {
            using var channel = RabbitMQConnection.Instance.Connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare(queue, exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue

            channel.BasicPublish(exchange: "", routingKey: queue, props, body: body);

        }
    }
}