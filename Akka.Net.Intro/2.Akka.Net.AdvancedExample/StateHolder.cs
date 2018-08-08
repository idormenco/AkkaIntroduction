using Akka.Util;

namespace Akka.Net.AdvancedExample
{
	public static class StateHolder
	{
		public static ConcurrentSet<string> StartedActors = new ConcurrentSet<string>();
	}
}