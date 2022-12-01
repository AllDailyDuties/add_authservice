﻿using RabbitMQ.Client;

namespace AllDailyDuties_AuthService.Middleware.Messaging
{
   public class RabbitMQConnection
   {
        private static readonly Lazy<RabbitMQConnection> Lazy = new Lazy<RabbitMQConnection>(() => new RabbitMQConnection());

        private RabbitMQConnection()
        {
            IConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            Connection = connectionFactory.CreateConnection();
        }

        public static RabbitMQConnection Instance
        {
            get { return Lazy.Value; }
        }

        public IConnection Connection { get; }
    }
}
