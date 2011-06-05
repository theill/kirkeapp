#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Xml;
#endregion

namespace dk.kirkeapp {
	partial class RootViewController : BackgroundViewController {
		public RootViewController(IntPtr handle) : base (handle) {
		}

		[Export ("initWithCoder:")]
		public RootViewController(NSCoder coder) : base (coder) {
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			NavigationItem.Title = appDelegate.ApplicationName;

			NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Konto", UIBarButtonItemStyle.Plain, AccountClick);

			NavigationController.NavigationBar.Alpha = 0.80f;
			NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(128, 128, 128, 172);

			UIImage image = UIImage.FromBundle("Images/paper-light.png");
			UIImageView a = new UIImageView(image);
			a.Frame = new System.Drawing.RectangleF(0, -64, 320, 480);
			this.View.AddSubview(a);
			this.View.SendSubviewToBack(a);

			this.ChurchNameLabel.Text = appDelegate.ApplicationName;

			btnMessages.TouchDown += delegate(object sender, EventArgs e) {
				Console.WriteLine("Displaying messages");
				var c = new MessagesViewController();
				NavigationController.PushViewController(c, true);
			};

			btnCalendar.TouchDown += delegate(object sender, EventArgs e) {
				Console.WriteLine("Displaying calendar");
				var c = new CalendarViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnDonation.TouchDown += delegate(object sender, EventArgs e) {
				Console.WriteLine("Displaying Donations");
				var c = new DonationsViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnAbout.TouchDown += (sender, e) => {
				Console.WriteLine("Displaying about");
				var c = new AboutViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnBible.TouchDown += (sender, e) => {
				Console.WriteLine("Displaying Bible");
				var c = new BibleViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnPsalms.TouchDown += (sender, e) => {
				Console.WriteLine("Displaying psalms");
				var c = new PsalmsViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnFavorites.TouchDown += (sender, e) => {
				Console.WriteLine("Displaying favorites");
				var c = new FavoritesViewController();
				NavigationController.PushViewController(c, true);
			};
		}

		void AccountClick(object sender, EventArgs e) {
			Console.WriteLine("Log in");
			var c = new LogOnViewController();
			this.NavigationController.PushViewController(c, false);
		}
		
		public override void DidReceiveMemoryWarning() {
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidUnload() {
			// Release anything that can be recreated in viewDidLoad or on demand.
			// e.g. this.myOutlet = null;
			
			base.ViewDidUnload();
		}
		
	}
	
}

