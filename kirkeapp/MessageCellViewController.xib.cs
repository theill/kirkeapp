#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class MessageCellViewController : UIViewController {
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for items that need
		// to be able to be created from a xib rather than from managed code

		public MessageCellViewController(IntPtr handle) : base (handle) {
			Initialize();
		}

		[Export ("initWithCoder:")]
		public MessageCellViewController(NSCoder coder) : base (coder) {
			Initialize();
		}

		public MessageCellViewController() : base ("MessageCellViewController", null) {
			Initialize();
		}

		void Initialize() {
		}

		#endregion

		public UITableViewCell ViewCell {
			get {
				return this.Cell;
			}
		}

		public void Configure(Dictionary<string, string> options) {
			this.SubjectLabel.Text = options["subject"];
			this.ContentLabel.Text = options["content"];
			this.DateLabel.Text = options["sent_at"];
		}
	}
}
