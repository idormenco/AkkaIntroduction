using System;
using Akka.Actor;

namespace _1.Akka.Net.BasicHelloWorld
{
    public class PrinterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            var msg = message as string;

            // make sure we got a message
            if (string.IsNullOrEmpty(msg))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Please provide an input.\n");
                Console.ResetColor();
                return;
            }

            // if message has even # characters, display in red; else, green
            var even = msg.Length % 2 == 0;
            var color = even ? ConsoleColor.Red : ConsoleColor.Green;

            Console.ForegroundColor = color;
            Console.WriteLine("user message" + message);
            Console.ResetColor();
        }
    }
}