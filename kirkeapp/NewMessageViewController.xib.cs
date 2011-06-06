#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class NewMessageViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public NewMessageViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public NewMessageViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public NewMessageViewController() : base ("NewMessageViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Ny besked";

			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Send", UIBarButtonItemStyle.Bordered, (sender, e) => {
				Console.WriteLine("Let's send this message");
				this.NavigationController.PopViewControllerAnimated(true);
			});

			this.MessageTextView.BecomeFirstResponder();
		}
	}
}