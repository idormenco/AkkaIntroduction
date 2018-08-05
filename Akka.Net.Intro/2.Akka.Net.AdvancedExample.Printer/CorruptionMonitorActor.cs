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
        private readonly Graph graph = new Graph();

        public CorruptionMonitorActor(GraphViewer viewer)
        {
            _viewer = viewer;

            Receive<PrintMeMessage>(x =>
            {

                if (_knownActors.ContainsKey(x.Name))
                {
                    graph.FindNode(x.Name);
                }
                else
                {
                    _knownActors.Add(x.Name, Sender);

                    var actors = x.Path.Replace("/user/", "").Split('/');
                    if (actors.Length == 1)
                    {
                        var node = graph.AddNode(x.Name);
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
                                var newEdge = graph.AddEdge(parrent, leaf);
                                newEdge.Attr.Id = edgeId;
                            }

                        }

                        //var node = viewerGraph.AddNode(x.Name);
                        //node.LabelText = x.Capital.ToString();
                    }

                    _viewer.Graph = graph;
                }

            });

            Receive<PrintBustedMessage>(x =>
            {

            });

        }
    }
}