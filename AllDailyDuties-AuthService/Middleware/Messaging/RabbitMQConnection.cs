using RabbitMQ.Client;

namespace AllDailyDuties_AuthService.Middleware.Messaging
{
   public class RabbitMQConnection
   {
        private static readonly Lazy<RabbitMQConnection> Lazy = new Lazy<RabbitMQConnection>(() => new RabbitMQConnection());

        private RabbitMQConnection()
        {
            IConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            IConnectionFactory connectionFactory2 = new ConnectionFactory
            {
                HostName = "rabbitmq-server-0",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            IConnectionFactory connectionFactory3 = new ConnectionFactory
            {
                HostName = "rabbitmq.default.svc.cluster.local",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            IConnectionFactory connectionFactory4 = new ConnectionFactory
            {
                HostName = "10.152.183.226",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            IConnectionFactory connectionFactory5 = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            List<IConnectionFactory> list = new List<IConnectionFactory>();
            list.Add(connectionFactory);
            list.Add(connectionFactory2);
            list.Add(connectionFactory3);
            list.Add(connectionFactory4);
            list.Add(connectionFactory5);
            foreach (var item in list)
            {
                try
                {
                    Connection = item.CreateConnection();
                    Console.WriteLine(Connection.ToString());
                    break;
                }
                catch (Exception)
                {

                }
            }
            //Connection = connectionFactory.CreateConnection();
            //Connection = connectionFactory5.CreateConnection();
        }

        public static RabbitMQConnection Instance
        {
            get { return Lazy.Value; }
        }

        public IConnection Connection { get; }
    }
}
