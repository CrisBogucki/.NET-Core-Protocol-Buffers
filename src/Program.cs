using System;
using System.IO;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Tutorial;

namespace RabbitMQProtocolBuffers
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "localhost", UserName = "guest", Password = "guest"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("MY_EXCHANGE_NAME", "topic", true);
                    var queueName = channel.QueueDeclare("TV Queue", true, false, false, null);


                    if (args.Length < 1)
                    {
                        Console.Error.WriteLine("Usage: {0} [binding_key...]",
                            Environment.GetCommandLineArgs()[0]);
                        Environment.ExitCode = 1;
                        return;
                    }

                    foreach (var bindingKey in args)
                    {
                        channel.QueueBind(queueName, "MY_EXCHANGE_NAME", bindingKey);
                    }

                    Console.WriteLine(" [*] Waiting for messages. " +
                                      "To exit press CTRL+C");

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs) consumer.Queue.Dequeue();
                        var routingKey = ea.RoutingKey;
                        var body = ea.Body;

                        MemoryStream stream = new MemoryStream(body, false);

                        stream.Position = 0;

                        var message = Serializer.Deserialize<Listing>(stream);


                        Console.WriteLine(" [x] Received '{0}':'{1}'",
                            routingKey, message.ToString());

                        Console.WriteLine("Program Name: {0}", message.ProgramName);
                        Console.WriteLine("Episode Name: {0}", message.EpisodeName);
                        Console.WriteLine("Episode Number: {0}", message.EpisodeNumber);
                        Console.WriteLine("Is Live: {0}", message.IsLive);
                    }
                }
            }
        }
    }
    
}