#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

#endregion

namespace dk.kirkeapp {
	public partial class MessageCellViewController : UIViewController, IJsonCellController {
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

		public MessageCellViewController() { //: base ("MessageCellViewController", null) {
			MonoTouch.Foundation.NSBundle.MainBundle.LoadNib("MessageCellViewController", this, null);
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

		public void Configure(OptionDictionary options) {
			this.SubjectLabel.Text = options.ContainsKey("From") ? (string)options["From"] : string.Empty;
			this.ContentLabel.Text = options.ContainsKey("Content") ? (string)options["Content"] : string.Empty;
			this.DateLabel.Text = (options.ContainsKey("SentAt") ? Convert.ToDateTime(options["SentAt"]) : DateTime.Now).ToString("d/M-yyyy");

			if (!string.IsNullOrEmpty(this.ContentLabel.Text)) {
				NSString a = new NSString(this.ContentLabel.Text);
				SizeF size = a.StringSize(this.ContentLabel.Font, new SizeF(280, 9999), this.ContentLabel.LineBreakMode);
				this.ContentLabel.Frame = new System.Drawing.RectangleF(this.ContentLabel.Frame.X, this.ContentLabel.Frame.Y, size.Width, size.Height);
			}
		}
	}
}