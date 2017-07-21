using System;
namespace Lounging
{
	public class LoungeMessage
	{
		public LoungeMessage(string jsonPayLoad, int messageType, bool isList = false)
		{
			JsonPayload = jsonPayLoad;
			LoungeMessageType = messageType;
			IsList = isList;
		}

		public string JsonPayload { get; set; }

		public int LoungeMessageType { get; set; }

		public bool IsList { get; set; }
	}

	public enum LoungeMessageType
	{
		LoungeDiscovery = 1,
	}
}
