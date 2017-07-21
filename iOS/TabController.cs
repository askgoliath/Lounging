using System;
using UIKit;

namespace Lounging
{
	public class TabController : UITabBarController
	{
		UIViewController tab1, tab2;
		public TabController()
		{
			var storyboard = UIStoryboard.FromName("Main", null);
			//var discoverView = storyboard?.InstantiateViewController("discoverView") as DiscoverViewController;
			var navigationController = storyboard?.InstantiateViewController("DiscoverNavigationController") as UINavigationController;
			//navigationController.PushViewController(discoverView, false);
			tab1 = navigationController;
			tab1.Title = "Discover";
			//tab1.View.BackgroundColor = UIColor.LightGray;

			tab2 = new UIViewController();
			tab2.Title = "Orange";
			tab2.View.BackgroundColor = UIColor.Orange;

			var tabs = new UIViewController[] {
				tab1, tab2
			};

			ViewControllers = tabs;
		}
	}
}
