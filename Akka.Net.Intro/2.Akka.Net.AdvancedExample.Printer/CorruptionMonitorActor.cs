using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Net.AdvancedExample.SharedMessages;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace Akka.Net.AdvancedExample.Printer
{
	public class CorruptionMonitorActor : ReceiveActor
	{
		private readonly GraphViewer _viewer;
		private readonly Dictionary<string, IActorRef> _knownActors = new Dictionary<string, IActorRef>();
		private readonly HashSet<string> _knownEdges = new HashSet<string>();
		private readonly Graph _graph;

		public CorruptionMonitorActor(GraphViewer viewer)
		{
			_viewer = viewer;
			_graph = viewer.Graph;

			ReceiveSetup();
		}

		private void ReceiveSetup()
		{
			Receive<PrintMeMessage>(x =>
			{
				if (_knownActors.ContainsKey(x.Name))
				{
					_graph.FindNode(x.Name);
					var senderNode = _graph.FindNode(x.Name);
					var displayCapital = x.Capital == 0 ? string.Empty : $"{Environment.NewLine}${x.Capital}";

					senderNode.LabelText = $"{x.Name}{displayCapital}";
				}
				else
				{
					_knownActors.Add(x.Name, Sender);
					var actors = x.Path.Replace("/user/", "").Split('/');
					if (actors.Length > 1)
					{
						for (int i = 0; i < actors.Length - 1; i++)
						{
							var parrent = actors[i];
							var leaf = actors[i + 1];
							var edgeId = $"{parrent}{leaf}";

							if (!_knownEdges.Contains(edgeId))
							{
								_knownEdges.Add(edgeId);
								var parrentNode = _graph.AddNode(parrent);
								var childNode = _graph.AddNode(leaf);

								var newEdge = _graph.AddEdge(parrentNode.Id, childNode.Id);
								newEdge.Attr.Id = edgeId;
								newEdge.Attr.ArrowheadAtTarget = ArrowStyle.None;
							}
						}

						var senderNode = _graph.FindNode(actors.Last());
						senderNode.Attr.FillColor = x.Gender == GenderEnum.Male ? Color.CadetBlue : Color.LightPink;
						senderNode.UserData = Sender;
					}

				}
				_viewer.Graph = _graph;

			});
			Receive<CatchThisOneMessage>(x =>
			{
				x.Actor.Tell(GothcaMessage.Instance);
			});

			Receive<PrintBustedMessage>(x => { });
		}

		public class CatchThisOneMessage
		{
			public IActorRef Actor { get; }

			public CatchThisOneMessage(IActorRef actor)
			{
				Actor = actor;
			}
		}
	}
}