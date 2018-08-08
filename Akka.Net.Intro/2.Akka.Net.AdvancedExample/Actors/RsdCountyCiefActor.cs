using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Net.AdvancedExample.SharedMessages;

namespace Akka.Net.AdvancedExample.Actors
{
	public class RsdCountyCiefActor : ReceiveActor
	{
		private readonly string _name;
		private readonly GenderEnum _gender;
		private int _privateCapital;
		private readonly IActorRef _printerActorRef;
		private readonly HashSet<IActorRef> _childActorRefs = new HashSet<IActorRef>();

		protected override SupervisorStrategy SupervisorStrategy()
		{
			return new OneForOneStrategy(
				maxNrOfRetries: 5,
				withinTimeRange: TimeSpan.FromMinutes(10),
				localOnlyDecider: ex =>
				{
					switch (ex)
					{
						default:
							return Directive.Stop;
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

		public RsdCountyCiefActor(string name, GenderEnum gender, IActorRef printerActorRef)
		{
			_name = name;
			_gender = gender;
			_printerActorRef = printerActorRef;

			Random r = new Random();
			var nrOfLocalityCiefs = r.Next(1, 4);
			for (int i = 0; i < nrOfLocalityCiefs; i++)
			{
				var g = r.Next(1, 3);
				var randomGender = (GenderEnum)g;
				var randomName = RomanianNameSurnameUtils.GetName(randomGender);
				var randomSurname = RomanianNameSurnameUtils.GetSurname();

				var childName = $"{randomSurname}~{randomName}";

				var childActorRef = Context.ActorOf(RsdLocalityCiefActor.Props(childName,
					randomGender,
					printerActorRef),
					childName);

				_childActorRefs.Add(childActorRef);
				Context.Watch(childActorRef);
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

			Receive<CollectTaxesMessage>(x =>
			{
				foreach (var worker in _childActorRefs)
				{
					worker.Tell(GimmyTaxMessage.Instance);
				}
			});

			Receive<PassTaxesToBossMessage>(x =>
			{
				var taxAmount = _privateCapital / 2;
				if (_privateCapital - taxAmount >= 0)
				{
					Context.Parent.Tell(new TaxIncomeMessage(taxAmount));
					_printerActorRef.Tell(new MoneyFlowMessage(Context.Parent.Path.Name, (int)taxAmount));
					_privateCapital -= taxAmount;
					Print();
				}
			});

			if (!StateHolder.StartedActors.Contains(_name))
			{
				StateHolder.StartedActors.TryAdd(_name);
				Print();
			}

			Receive<Terminated>(t =>
			{
				_childActorRefs.Remove(Sender);

			});

			Context.System.Scheduler
				.ScheduleTellRepeatedly(TimeSpan.FromSeconds(30),
					TimeSpan.FromSeconds(30),
					Self,
					CollectTaxesMessage.Instance,
					ActorRefs.Nobody);

			Context.System.Scheduler
				.ScheduleTellRepeatedly(TimeSpan.FromSeconds(45),
					TimeSpan.FromSeconds(45),
					Context.Self,
					PassTaxesToBossMessage.Instance,
					ActorRefs.Nobody);
		}

		public static Props Props(string name, GenderEnum gender, IActorRef printerActorRef)
		{
			return Actor.Props.Create(() => new RsdCountyCiefActor(name, gender, printerActorRef));
		}

		private class PassTaxesToBossMessage
		{
			public static PassTaxesToBossMessage Instance => new PassTaxesToBossMessage();
		}
	}
}