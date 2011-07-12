#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

#endregion

namespace dk.kirkeapp {
	public partial class BackgroundViewController : UIViewController {
		public BackgroundViewController(IntPtr handle) : base (handle) {
		}

		[Export ("initWithCoder:")]
		public BackgroundViewController(NSCoder coder) : base (coder) {
		}

		public BackgroundViewController(string nibName, object o) : base (nibName, null) {

		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			UIImage image = UIImage.FromBundle("Images/wood-bg.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.SendSubviewToBack(a);
		}
	}
}

