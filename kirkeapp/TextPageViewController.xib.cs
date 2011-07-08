#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class TextPageViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public TextPageViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public TextPageViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public TextPageViewController() : base ("TextPageViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public string Text {
			get;
			set;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			NavigationItem.Title = this.Title;

			this.TextView.Text = this.Text;
		}
	}
}

