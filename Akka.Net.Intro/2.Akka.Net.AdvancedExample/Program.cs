using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.Net.AdvancedExample.Actors;

namespace Akka.Net.AdvancedExample
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("try connect tot actor system");

			var remoteConfig = ConfigurationFactory.ParseString(@"
                akka {
                    actor {
                        provider = remote
                    }
                    remote {
                        dot-netty.tcp {
                            port = 0
                            hostname = localhost
                        }
                    }
	             }
            ");

			var clusterConfig = ConfigurationFactory.ParseString(@"
            akka {
                actor {
                    provider = cluster

				}
                remote {
                    dot-netty.tcp {
                        port = 0
                        hostname = localhost
                      }
                }
				cluster {
                        seed-nodes = [""akka.tcp://RomaniaSimulationActorSystem@localhost:8081""]
                }
            }
            ");

			var rsdPartyActorSystem = ActorSystem.Create("RomaniaSimulationActorSystem", remoteConfig);

			var printer = rsdPartyActorSystem
				.ActorSelection("akka.tcp://RomaniaSimulationActorSystem@localhost:8081/user/Printer");

			var printerRef = printer.ResolveOne(TimeSpan.FromSeconds(30)).Result;

			rsdPartyActorSystem
				.ActorOf(RsdBossActor.Props("Bragnea", printerRef), "Bragnea");

			rsdPartyActorSystem.WhenTerminated.Wait();
		}
	}
}
