#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

#endregion

namespace dk.kirkeapp {
	public partial class DonationsViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public DonationsViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public DonationsViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public DonationsViewController() : base ("DonationsViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion
		
		public override void ViewDidLoad() {
			base.ViewDidLoad();

			NavigationItem.Title = "Kirkebøssen";

			UIImage image = UIImage.FromBundle("Images/paper-light.png");
			UIImageView a = new UIImageView(image);
			a.Frame = new System.Drawing.RectangleF(0, -64f, 320, 480);
			this.View.AddSubview(a);
			this.View.SendSubviewToBack(a);

			DraggableImageView img = new DraggableImageView(new RectangleF(40, 30, 50, 50));
			img.Image = UIImage.FromBundle("Images/single-coin.png");
			img.Opaque = true;
			img.UserInteractionEnabled = true;
			img.Hidden = false;
			img.OnDroppedImage += (location) => {
				Log.WriteLine("Dropped at {0}", location);
				Log.WriteLine("Where is box image {0}", this.BoxImageView.Frame);
				if (this.BoxImageView.Frame.Contains(location)) {
					InvokeOnMainThread(delegate {
						UIAlertView v = new UIAlertView("Tak", "Ønsker du at donere 25 DKK til kirken?", null, "Nej", "Ja, tak");
						v.Show();
					});
				}
			};

			this.View.AddSubview(img);
		}
	}
}

