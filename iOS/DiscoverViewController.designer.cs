// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Lounging
{
    [Register ("DiscoverViewController")]
    partial class DiscoverViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Lounging.LoungesTableView loungesView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem NewLoungeButton { get; set; }

        [Action ("NewLoungeButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NewLoungeButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (loungesView != null) {
                loungesView.Dispose ();
                loungesView = null;
            }

            if (NewLoungeButton != null) {
                NewLoungeButton.Dispose ();
                NewLoungeButton = null;
            }
        }
    }
}