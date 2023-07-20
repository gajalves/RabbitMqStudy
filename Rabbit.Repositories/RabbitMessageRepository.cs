using Rabbit.Models.Entities;
using Rabbit.Repositories.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Rabbit.Repositories
{
    public class RabbitMessageRepository : IRabbitMessageRepository
    {
        public void SendMessage(RabbitMessage rabbitMessage)
        {
            var factory = new ConnectionFactory { 
                HostName = "localhost" ,
                UserName= "admin" ,
                Password = "123456"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "RabbitMessagesQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            
            string message = JsonSerializer.Serialize(rabbitMessage);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "RabbitMessagesQueue",
                                 basicProperties: null,
                                 body: body);            
        }
    }
}
