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

		protected override SupervisorStrategy SupervisorStrategy()
		{
			return new OneForOneStrategy(
				maxNrOfRetries: 2,
				withinTimeRange: TimeSpan.FromMinutes(5),
				
				localOnlyDecider: ex =>
				{
					switch (ex)
					{
						default:
							return Directive.Restart;
					}
				});
		}

		public override void AroundPostStop()
		{
			_printerActorRef.Tell(new IAmDeadMessage(_name));
		}

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
			var nrOfWorkers = r.Next(2, 6);
			for (int i = 0; i < nrOfWorkers; i++)
			{

				var g = r.Next(1, 3);
				var randomGender = (GenderEnum)g;
				var randomName = RomanianNameSurnameUtils.GetName(randomGender);
				var randomSurname = RomanianNameSurnameUtils.GetSurname();

				var childName = $"{randomSurname}~{randomName}";
				var workerProps = RsdSimpleWorkerActor.Props(childName, randomGender, printerActorRef);
				var childActorRef = Context.ActorOf(workerProps, childName);
				_childActorRefs.Add(childActorRef);
				Context.Watch(childActorRef);
			}

			if (!StateHolder.StartedActors.Contains(_name))
			{
				StateHolder.StartedActors.TryAdd(_name);
				Print();
			}

			Receive<TaxIncomeMessage>(x =>
			{
				_privateCapital += x.Amount;
				Print();
			});

			Receive<GimmyTaxMessage>(x =>
			{
				if (Sender.Equals(Context.Parent))
				{
					var taxAmount = _privateCapital * 0.65;
					if (_privateCapital - taxAmount >= 0)
					{
						Sender.Tell(new TaxIncomeMessage((int) taxAmount));
						_printerActorRef.Tell(new MoneyFlowMessage(Context.Parent.Path.Name, (int)taxAmount));
						_privateCapital -= (int)taxAmount;
						Print();
					}
				}
			});
			Receive<Terminated>(t =>
			{
				_childActorRefs.Remove(Sender);

			});
			Receive<GothcaMessage>(x =>
			{
				printerActorRef.Tell(new PrintBustedMessage(_name));
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