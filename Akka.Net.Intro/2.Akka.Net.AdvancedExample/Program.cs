
using System;
using Akka.Actor;
using Akka.Net.AdvancedExample.Actors;

namespace Akka.Net.AdvancedExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating acotor system");
            // make an actor system 
            RsdPartyActorSystem = ActorSystem.Create("RsdPartyActorSystem");

            var printerActorRef = RsdPartyActorSystem.ActorOf(ConsolePrinterActor.Props());
            IActorRef partyBossActor = RsdPartyActorSystem.ActorOf(RsdBossActor.Props("Bragnea", printerActorRef),
                "Bragnea");

            // blocks the main thread from exiting until the actor system is shut down
            RsdPartyActorSystem.WhenTerminated.Wait();
        }

        public static ActorSystem RsdPartyActorSystem { get; set; }
    }
}
