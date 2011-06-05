#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class TitleCellViewController : UIViewController, IJsonCellController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public TitleCellViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public TitleCellViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public TitleCellViewController() { //: base ("TitleCellViewController", null) {
			MonoTouch.Foundation.NSBundle.MainBundle.LoadNib("TitleCellViewController", this, null);
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public UITableViewCell ViewCell {
			get {
				return this.MainCell;
			}
		}

		public void Configure(OptionDictionary options) {
			this.TitleLabel.Text = options.ContainsKey("Title") ? (string)options["Title"] : string.Empty;
		}
	}
}

