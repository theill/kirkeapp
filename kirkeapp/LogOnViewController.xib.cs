using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace dk.kirkeapp {
	public partial class LogOnViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public LogOnViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public LogOnViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public LogOnViewController() : base ("LogOnViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationController.NavigationBar.Hidden = true;



		}
	}
}

