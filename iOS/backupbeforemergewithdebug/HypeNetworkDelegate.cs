using System;
using Foundation;
using HypeFramework;

namespace Lounging.iOS
{
	public class HypeNetworkDelegate: HypeFramework.HYPNetworkObserver
	{
		public HypeNetworkDelegate()
		{
		}

		public override void DidFindInstance(HYP hype, HYPInstance instance)
		{
			Console.WriteLine(instance.StringIdentifier);
		}

		public override void DidLoseInstance(HYP hype, HYPInstance instance, NSError error)
		{
			Console.WriteLine(error.DebugDescription);
		}
	}
}

