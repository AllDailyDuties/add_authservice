﻿using RabbitMQ.Client;

namespace AllDailyDuties_AuthService.Middleware.Messaging
{
    public interface IRabbitMQProducer
    {
        public void SendMessage<T>(T message, string queue, IBasicProperties props);
    }
}
