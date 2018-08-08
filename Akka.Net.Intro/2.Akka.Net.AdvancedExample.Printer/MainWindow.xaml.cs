using System.Linq;
using System.Windows;
using Akka.Actor;
using Akka.Configuration;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace Akka.Net.AdvancedExample.Printer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ActorSystem actorSystem;
		private IActorRef _corruptionMonitorActor;
		private readonly GraphViewer _viewer = new GraphViewer();

		public MainWindow()
		{
			var remoteConfig = ConfigurationFactory.ParseString(@"
            akka {
                actor {
                    provider = remote
				}
                remote {
                    dot-netty.tcp {
                        port = 8081
                        hostname = localhost
                      }
                }
            }
            ");

			var clusterConfig = ConfigurationFactory.ParseString(@"
            akka {
                actor {
                    provider = cluster

				}
                remote {
                    dot-netty.tcp {
                        port = 8081
                        hostname = localhost
                      }
                }
				cluster {
                        seed-nodes = [""akka.tcp://RomaniaSimulationActorSystem@localhost:8081""]
                }
            }
            ");

			actorSystem = ActorSystem.Create("RomaniaSimulationActorSystem", clusterConfig);

			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_viewer.BindToPanel(Panel);
			Graph graph = new Graph
			{
				Attr =
				{
					LayerDirection = LayerDirection.TB,
					OptimizeLabelPositions = true
				}
			};

			_viewer.Graph = graph;

			_corruptionMonitorActor = actorSystem
				.ActorOf(Props.Create(() => new CorruptionMonitorActor(_viewer))
					.WithDispatcher("akka.actor.synchronized-dispatcher"), "Printer");
		}

		private void KillButton_OnClick(object sender, RoutedEventArgs e)
		{
			foreach (var en in _viewer.Entities)
			{
				if (en.MarkedForDragging && en is IViewerNode)
				{
					var viewerNode = en as IViewerNode;
					var actorName = viewerNode.Node.Id;

					_corruptionMonitorActor.Tell(new CorruptionMonitorActor.CatchThisOneMessage(actorName));
					en.MarkedForDragging = false;
					
				}
			}

			_viewer.Entities.First().MarkedForDragging = true;
		}
	}
}

