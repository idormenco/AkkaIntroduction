using System;
using Akka.Actor;

namespace _1.Akka.Net.BasicHelloWorld
{
    public class ReaderActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var userInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(userInput)
                && string.Equals(userInput, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }
            _consoleWriterActor.Tell(userInput);

            Self.Tell("continue");
        }

        public string ExitCommand = "exit";
    }
}