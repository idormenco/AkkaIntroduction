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

        public MainWindow()
        {
            var config = ConfigurationFactory.ParseString(@"
            akka {
                actor {
                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                }
                remote {
                    dot-netty.tcp {
                        port = 8082
                        hostname = localhost
                      }
                }
            }
            ");

            actorSystem = ActorSystem.Create("RomaniaSimulationActorSystem", config);

            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GraphViewer viewer = new GraphViewer();
            viewer.BindToPanel(Panel);
            Graph graph = new Graph
            {
                Attr =
                {
                    LayerDirection = LayerDirection.TB
                }
            };

            viewer.Graph = graph;

            _corruptionMonitorActor = actorSystem
                .ActorOf(Props.Create(() => new CorruptionMonitorActor(viewer))
                    .WithDispatcher("akka.actor.synchronized-dispatcher"), "Printer");
        }

        private void KillButton_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}

