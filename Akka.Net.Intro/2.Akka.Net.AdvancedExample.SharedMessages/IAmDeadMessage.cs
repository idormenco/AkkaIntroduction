namespace Akka.Net.AdvancedExample.SharedMessages
{
	public class IAmDeadMessage
	{
		public IAmDeadMessage(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}
}