using Foundation;
using System;
using UIKit;
using HypeFramework;

namespace Lounging.iOS
{
    public partial class MainViewController : UIViewController
    {
		private HYP hype;
		private HypeNetworkDelegate networkObserver;
        public MainViewController (IntPtr handle) : base (handle)
        {
			hype = new HYP();
			networkObserver = new HypeNetworkDelegate();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			RequestHypeToStart();
		}
	
		private void RequestHypeToStart()
		{
			hype.StartWithOptions(Constants.HYPOptionIdentifierKey);
			hype.AddNetworkObserver(networkObserver);
		}
    }
}