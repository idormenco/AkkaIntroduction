using System;
using Akka.Actor;
using Akka.Net.AdvancedExample.SharedMessages;

namespace Akka.Net.AdvancedExample.Actors
{
    public class RsdBossActor : ReceiveActor
    {
        private readonly string _name;
        private int _privateCapital;
        private readonly IActorRef _printerActorRef;

        private void Print()
        {
            var printMeMessage = new PrintMeMessage(_name,
                GenderEnum.Male,
                _privateCapital,
                Self.Path.ToStringWithoutAddress());

            _printerActorRef.Tell(printMeMessage);
        }

        public RsdBossActor(string name, IActorRef printerActorRef)
        {
            _name = name;
            _printerActorRef = printerActorRef;

            Random r = new Random();
            //var nrOfLocalityCiefs = r.Next(1, 4);
            var nrOfLocalityCiefs = 2;
            for (int i = 0; i < nrOfLocalityCiefs; i++)
            {
                var g = r.Next(1, 3);
                Context.ActorOf(RsdCountyCiefActor.Props($"CC{i}",
                        (GenderEnum)g,
                        printerActorRef),
                    $"CC{i}");
            }

            Receive<TaxIncomeMessage>(x =>
            {
                _privateCapital += x.Amount;
                Print();
            });

            Receive<GothcaMessage>(x =>
            {
                printerActorRef.Tell(new PrintBustedMessage(Self.Path.ToStringWithoutAddress()));
                throw new GotMeException("Ho my god they cought me");
            });
        }

        public static Props Props(string name,IActorRef printerActorRef)
        {
            return Actor.Props.Create(() => new RsdBossActor(name, printerActorRef));
        }
    }
}