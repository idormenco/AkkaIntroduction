using System;

namespace Akka.Net.AdvancedExample
{
    public class GotMeException:Exception
    {
        public GotMeException()
        {
        }

        public GotMeException(string message) : base(message)
        {
        }
    }
}