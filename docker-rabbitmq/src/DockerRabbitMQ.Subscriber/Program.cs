using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DockerRabbitMQ.Subscriber
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            const string queueName = "testqueue";

            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    RequestedConnectionTimeout = 3000
                };

                using (var rabbitConnection = connectionFactory.CreateConnection())
                {
                    using (var channel = rabbitConnection.CreateModel())
                    {
                        // Declaring a queue is idempotent 
                        channel.QueueDeclare(
                            queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (ch, ea) =>
                        {
                            var body = ea.Body;
                           Console.WriteLine("Message Received: " + Encoding.UTF8.GetString(body));
                            channel.BasicAck(ea.DeliveryTag, false);
                        };

                        var consumerTag = channel.BasicConsume(queueName, false, consumer);
                        Console.WriteLine("consumerTag: " + consumerTag);
                        
                        do
                        {
                            Console.WriteLine("Press Q to exit");
                        }
                        while (Console.ReadKey().Key != ConsoleKey.Q);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("Exit");
        }
    }
}