namespace Akka.Net.AdvancedExample.SharedMessages
{
    public class PrintBustedMessage
    {
        public string Path { get; }

        public PrintBustedMessage(string path)
        {
            Path = path;
        }
    }
}