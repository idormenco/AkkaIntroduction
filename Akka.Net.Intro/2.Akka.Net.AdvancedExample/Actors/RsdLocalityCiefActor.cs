using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Net.AdvancedExample.SharedMessages;

namespace Akka.Net.AdvancedExample.Actors
{
    public class RsdLocalityCiefActor : ReceiveActor
    {
        private readonly string _name;
        private readonly GenderEnum _gender;
        private int _privateCapital;
        private readonly IActorRef _printerActorRef;
        private readonly HashSet<IActorRef> _childActorRefs = new HashSet<IActorRef>();

        private void Print()
        {
            var printMeMessage = new PrintMeMessage(_name,
                _gender,
                _privateCapital,
                Self.Path.ToStringWithoutAddress());

            _printerActorRef.Tell(printMeMessage);
        }

        public RsdLocalityCiefActor(string name, GenderEnum gender, IActorRef printerActorRef)
        {
            _name = name;
            _gender = gender;
            _printerActorRef = printerActorRef;

            Random r = new Random();
            //var nrOfWorkers = r.Next(2, 6);
            var nrOfWorkers = 2;
            for (int i = 0; i < nrOfWorkers; i++)
            {

                var g = r.Next(1, 3);
                var randomGender = (GenderEnum)g;
                var randomName = RomanianNameSurnameUtils.GetName(randomGender);
                var randomSurname = RomanianNameSurnameUtils.GetSurname();

                var childName = $"{randomSurname}~{randomName}";
				var workerProps = RsdSimpleWorkerActor.Props(childName, randomGender, printerActorRef);
                var actorRef = Context.ActorOf(workerProps, childName);
                _childActorRefs.Add(actorRef);
            }

            Print();

            Receive<TaxIncomeMessage>(x =>
            {
                _privateCapital += x.Amount;
                Print();
            });
            
            Receive<GimmyTaxMessage>(x =>
            {
                if (Sender.Equals(Context.Parent))
                {
                    var taxAmount = _privateCapital *0.65;
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

            Receive<CollectTaxesMessage>(x =>
            {
                foreach (var worker in _childActorRefs)
                {
                    worker.Tell(GimmyTaxMessage.Instance);
                }
            });

            Context.System.Scheduler
                .ScheduleTellRepeatedly(TimeSpan.FromSeconds(20),
                    TimeSpan.FromSeconds(20),
                    Self,
                    CollectTaxesMessage.Instance,
                    ActorRefs.Nobody);
        }

        public static Props Props(string name, GenderEnum gender, IActorRef printActorRef)
        {
            return Actor.Props.Create(() => new RsdLocalityCiefActor(name, gender, printActorRef));
        }

    }
}