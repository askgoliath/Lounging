using Foundation;
using System;
using UIKit;

namespace Lounging
{
    public partial class LoungeDetailViewController : UITableViewController
    {
		DiscoverViewController discoverViewController { get; set; }
		Lounge thisLounge;
        public LoungeDetailViewController (IntPtr handle) : base (handle)
        {
        }

		public void SetTask(DiscoverViewController controller, Lounge lounge)
		{
			discoverViewController = controller;
			thisLounge = lounge;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			CreateLoungeButton.TouchUpInside += (sender, e) =>
			{
				thisLounge.Name = LoungeNameField.Text;
				thisLounge.Description = LoungeDescriptionField.Text;
				discoverViewController.CreateNewLounge(thisLounge);
			};

			CancelLoungeButton.TouchUpInside += (sender, e) => discoverViewController.Cancel();
		}
	}
}