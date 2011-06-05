using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace dk.kirkeapp {
	public partial class EventCellViewController : UIViewController, IJsonCellController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public EventCellViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public EventCellViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public EventCellViewController() {
			MonoTouch.Foundation.NSBundle.MainBundle.LoadNib("EventCellViewController", this, null);
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
			this.ActiveAtLabel.Text = (options.ContainsKey("ActiveAt") ? Convert.ToDateTime(options["ActiveAt"]) : DateTime.Now).ToString("d/M-yyyy HH:mm");
		}

	}
}

