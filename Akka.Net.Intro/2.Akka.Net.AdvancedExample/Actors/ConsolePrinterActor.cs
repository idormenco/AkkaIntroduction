using System;
using Akka.Actor;
using Akka.Net.AdvancedExample.SharedMessages;

namespace Akka.Net.AdvancedExample.Actors
{
    public class ConsolePrinterActor:ReceiveActor
    {
        public ConsolePrinterActor()
        {
            Receive<PrintMeMessage>(x =>
            {
                Console.WriteLine($"{x.Path} {x.Name} {x.Gender} {x.Capital}");
            });
        }


        public static Props Props()
        {
            return Actor.Props.Create(() => new ConsolePrinterActor());
        }
    }
}