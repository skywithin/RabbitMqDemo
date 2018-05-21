using Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using EasyNetQ;

namespace Receiver
{
    class Receive
    {
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus(
                GetConnectionString(),
                registerServices: x => { }))
            {
                bus.Subscribe<TextMessage>("testSubscription", HandleTextMessage);

                Console.WriteLine("Listening for messages. Press [return] to exit.");
                Console.ReadLine();
            }
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

        static void HandleTextMessage(TextMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Got message: {0}", textMessage.Message);
            Console.ResetColor();
        }
    }
}
