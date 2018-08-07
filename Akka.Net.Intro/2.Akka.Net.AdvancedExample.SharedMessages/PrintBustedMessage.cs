namespace Akka.Net.AdvancedExample.SharedMessages
{
    public class PrintBustedMessage
    {
        public string Name { get;  }

        public PrintBustedMessage(string name)
        {
            Name = name;
        }
    }
}