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
            using (var bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest"))
            {
                bus.Subscribe<TextMessage>("testSubscription", HandleTextMessage);

                Console.WriteLine("Listening for messages. Press [return] to exit.");
                Console.ReadLine();
            }
        }

        static void HandleTextMessage(TextMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Got message: {0}", textMessage.Message);
            Console.ResetColor();
        }
    }
}
