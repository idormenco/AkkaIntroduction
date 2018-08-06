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
			//var nrOfLocalityCiefs = r.Next(1, 4);
			var nrOfLocalityCiefs = 2;
			for (int i = 0; i < nrOfLocalityCiefs; i++)
			{
				var g = r.Next(1, 3);
				var randomGender = (GenderEnum)g;
				var randomName = RomanianNameSurnameUtils.GetName(randomGender);
				var randomSurname = RomanianNameSurnameUtils.GetSurname();

				var childName = $"{randomSurname}~{randomName}";

				var actorRef = Context.ActorOf(RsdLocalityCiefActor.Props(childName,
					randomGender,
					printerActorRef),
					childName);

				_childActorRefs.Add(actorRef);
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
					_privateCapital -= taxAmount;
					Print();
				}
			});

			Print();

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