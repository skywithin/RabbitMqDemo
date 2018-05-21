using EasyNetQ;
using RabbitMQ.Client;
using System;
using System.Text;
using Domain;
using System.Threading;

namespace Sender
{
    class Send
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                var msgText = string.Format("Loop {0}", i);
                var msg = new TextMessage() { Message = msgText };

                Console.WriteLine("Sending message: {0}", msgText);

                TryPublishWithLimitedRetries(msg);

                Thread.Sleep(10);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        static string GetConnectionString()
        {
            return string.Format(
                "host={0}:{1};virtualHost={2};username={3};password={4};requestedHeartbeat={5};publisherConfirms=true",
                "localhost", //mq cluster:"mqTest01,mqTest02"
                5672, //port
                "/",
                "guest",
                "guest",
                0); //requestedHeartbeat
        }

        static void TryPublishWithLimitedRetries(TextMessage msg)
        {
            int[] delaysInMilliseconds = new[] { 1000, 2000, 4000 };

            for (int i = 0; i <= delaysInMilliseconds.Length; i++)
            {
                try
                {
                    using (var bus = RabbitHutch.CreateBus(
                        GetConnectionString(),
                        registerServices: x => { }))
                    {
                        bus.Publish(msg);
                    }

                    // Message published successfully.
                    return;
                }
                catch (Exception ex)
                {
                    if (i >= delaysInMilliseconds.Length)
                    {
                        // Number of retries exceeded.
                        Console.WriteLine("Failed to publish to queue");
                        Console.WriteLine(ex);
                        throw;
                    }

                    var delay = delaysInMilliseconds[i];
                    Console.WriteLine("Failed to publish to queue. Retry in {0} ms.", delay);
                    
                    Thread.Sleep(delay);
                }
            }
        }
    }
}
