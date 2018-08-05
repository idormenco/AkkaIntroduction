using System;
using System.Threading;
using Akka.Actor;
using Akka.Configuration;
using Akka.Net.AdvancedExample.Actors;
using Akka.Net.AdvancedExample.SharedMessages;

namespace Akka.Net.AdvancedExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("try connect tot actor system");

            var config = ConfigurationFactory.ParseString(@"
                akka {
                    actor {
                        provider = remote
                    }
                    remote {
                        dot-netty.tcp {
                            port = 8081
                            hostname = 0.0.0.0
                            public-hostname = localhost
                        }
                    }
                }
            ");

            var rsdPartyActorSystem = ActorSystem.Create("RomaniaSimulationActorSystem", config);

            var printer = rsdPartyActorSystem
                .ActorSelection("akka.tcp://RomaniaSimulationActorSystem@localhost:8082/user/Printer");

            var printerRef = printer.ResolveOne(TimeSpan.FromSeconds(30)).Result;
            //for (int i = 0; i < 10; i++)
            //{
            //    printer.Tell(new PrintMeMessage("", GenderEnum.Female, 1, "dd"));
            //    Thread.Sleep(100);
            //}

            rsdPartyActorSystem
                .ActorOf(RsdBossActor.Props("Bragnea", printerRef), "Bragnea");

            rsdPartyActorSystem.WhenTerminated.Wait();
        }
    }
}
