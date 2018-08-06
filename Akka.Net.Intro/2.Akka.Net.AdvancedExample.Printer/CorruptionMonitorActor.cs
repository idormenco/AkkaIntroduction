using System.Collections.Generic;
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
                }
                else
                {
                    _knownActors.Add(x.Name, Sender);
                    var actors = x.Path.Replace("/user/", "").Split('/');
                    if (actors.Length == 1)
                    {
                        //var node = graph.AddNode(x.Name);
                        //node.LabelText = x.Capital.ToString();
                    }
                    else
                    {
                        for (int i = 0; i < actors.Length - 1; i++)
                        {
                            var parrent = actors[i];
                            var leaf = actors[i + 1];
                            var edgeId = $"{parrent}{leaf}";

                            if (!_knownEdges.Contains(edgeId))
                            {
                                _knownEdges.Add(edgeId);
                                var newEdge = _graph.AddEdge(parrent, leaf);
                                newEdge.Attr.Id = edgeId;
                            }
                        }
                    }

                }
                _viewer.Graph = _graph;

            });

            Receive<PrintBustedMessage>(x => { });
        }
    }
}