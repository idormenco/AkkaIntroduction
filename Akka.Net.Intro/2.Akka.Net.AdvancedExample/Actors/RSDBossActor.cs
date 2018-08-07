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

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new AllForOneStrategy(
                maxNrOfRetries: 5,
                withinTimeRange: TimeSpan.FromMinutes(10),
                localOnlyDecider: ex =>
                {
                    switch (ex)
                    {
                        case GotMeException ge:
                            return Directive.Restart;
                        default:
                            return Directive.Stop;
                    }
                });
        }

	    public override void AroundPostRestart(Exception cause, object message)
	    {
		    Print();
	    }

	    public override void AroundPostStop()
	    {
		    _printerActorRef.Tell(new IAmDeadMessage(_name));
	    }

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
            var nrOfLocalityCiefs = 1;
            for (int i = 0; i < nrOfLocalityCiefs; i++)
            {
                var g = r.Next(1, 3);
                var randomGender = (GenderEnum)g;
                var randomName = RomanianNameSurnameUtils.GetName(randomGender);
                var randomSurname = RomanianNameSurnameUtils.GetSurname();

                var childName = $"{randomSurname}~{randomName}";

                Context.ActorOf(RsdCountyCiefActor.Props(childName,randomGender,printerActorRef),childName);
            }

            Receive<TaxIncomeMessage>(x =>
            {
                _privateCapital += x.Amount;
                Print();
            });

            Receive<GothcaMessage>(x =>
            {
                printerActorRef.Tell(new PrintBustedMessage(_name));
                throw new GotMeException("Ho my god they cought me");
            });
        }

        public static Props Props(string name,IActorRef printerActorRef)
        {
            return Actor.Props.Create(() => new RsdBossActor(name, printerActorRef));
        }
    }
}