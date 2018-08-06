using System;
using Akka.Actor;
using Akka.Net.AdvancedExample.SharedMessages;

namespace Akka.Net.AdvancedExample.Actors
{
    public class RsdSimpleWorkerActor : ReceiveActor
    {
        private readonly string _name;
        private readonly GenderEnum _gender;
        private int _privateCapital;
        private readonly IActorRef _printerActorRef;

        private class StealMessage
        {
            public static StealMessage Instance => new StealMessage();
        }

        public RsdSimpleWorkerActor(string name, GenderEnum gender, IActorRef printerActorRef)
        {
            _name = name;
            _gender = gender;
            _printerActorRef = printerActorRef;

            var rnd = new Random();
            Receive<StealMessage>(x =>
            {
                int stealedCapital = rnd.Next(10, 50);
                _privateCapital += stealedCapital;
                Print();
            });

            Receive<GimmyTaxMessage>(x =>
            {
                if (Sender.Equals(Context.Parent))
                {
                    var taxAmount = _privateCapital*0.75;
                    if (_privateCapital - taxAmount >= 0)
                    {
                        Sender.Tell(new TaxIncomeMessage((int)taxAmount));
                        _privateCapital -= (int)taxAmount;
                        Print();
                    }
                }
            });

            Receive<GothcaMessage>(x =>
            {
                printerActorRef.Tell(new PrintBustedMessage(Self.Path.ToStringWithoutAddress()));
                throw new GotMeException("Ho my god they cought me");
            });

            Print();

            Context.System.Scheduler
                .ScheduleTellRepeatedly(TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(10),
                    Self,
                    StealMessage.Instance,
                    ActorRefs.Nobody);

        }

        private void Print()
        {
            var printMeMessage = new PrintMeMessage(_name,
                _gender,
                _privateCapital,
                Self.Path.ToStringWithoutAddress());

            _printerActorRef.Tell(printMeMessage);
        }


        public static Props Props(string name, GenderEnum gender, IActorRef printeActorRef)
        {
            return Actor.Props.Create(() => new RsdSimpleWorkerActor(name, gender, printeActorRef));
        }
    }
}