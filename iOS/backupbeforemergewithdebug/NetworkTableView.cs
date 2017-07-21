//using Foundation;
//using System;
//using UIKit;
//using HypeFramework;

//namespace Lounging.iOS
//{
//    public partial class NetworkTableView : UITableView
//    {
//		private HYP hype;
//		private HypeNetworkDelegate networkObserver;

//		public NetworkTableView (IntPtr handle) : base (handle)
//        {
//			hype = new HYP();
//			networkObserver = new HypeNetworkDelegate();
//			RequestHypeToStart();
//        }
 
//		void RequestHypeToStart()
//		{
//			hype.StartWithOptions(Constants.HYPOptionIdentifierKey);
//			hype.AddNetworkObserver(networkObserver);
//		}
//    }
//}