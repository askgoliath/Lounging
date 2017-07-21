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
    [Register ("LoungeDetailViewController")]
    partial class LoungeDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelLoungeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CreateLoungeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView LoungeDescriptionField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView LoungeDetailView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LoungeNameField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CancelLoungeButton != null) {
                CancelLoungeButton.Dispose ();
                CancelLoungeButton = null;
            }

            if (CreateLoungeButton != null) {
                CreateLoungeButton.Dispose ();
                CreateLoungeButton = null;
            }

            if (LoungeDescriptionField != null) {
                LoungeDescriptionField.Dispose ();
                LoungeDescriptionField = null;
            }

            if (LoungeDetailView != null) {
                LoungeDetailView.Dispose ();
                LoungeDetailView = null;
            }

            if (LoungeNameField != null) {
                LoungeNameField.Dispose ();
                LoungeNameField = null;
            }
        }
    }
}