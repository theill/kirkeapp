#region Using directives
using System;
using System.Drawing;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

#endregion

namespace dk.kirkeapp {
	[MonoTouch.Foundation.Register("ImageNavigationBar")]
	public class ImageNavigationBar : UINavigationBar {
		
		public ImageNavigationBar() : base() {
			
		}
		
		public ImageNavigationBar(NSCoder coder) : base(coder) {
			
		}

		public ImageNavigationBar(IntPtr ptr) : base(ptr) {
			
		}

		public ImageNavigationBar(NSObjectFlag t) : base(t) {
			
		}
		
		public ImageNavigationBar(RectangleF frame) : base(frame) {

		}

		public override void Draw(RectangleF rect) {
			base.Draw(rect);
			
//			UIImage image = UIImage.FromBundle("Images/navigationbar.png");
//			image.Draw(rect);
		}
	}
}