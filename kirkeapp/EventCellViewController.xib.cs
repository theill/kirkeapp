#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using dk.kirkeapp.data;

#endregion

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

			string active = string.Empty;

			DateTime activeStartAt = options.ContainsKey("ActiveStartAt") ? Convert.ToDateTime(options["ActiveStartAt"]) : DateTime.Now;
			if (activeStartAt.Hour == 0 && activeStartAt.Minute == 0) {
				active = activeStartAt.ToString("d/M-yyyy");
			} else {
				active = activeStartAt.ToString("d/M-yyyy HH:mm");
			}

			DateTime activeEndAt = options.ContainsKey("ActiveEndAt") ? Convert.ToDateTime(options["ActiveEndAt"]) : DateTime.Now;
			if (activeStartAt != activeEndAt) {
				if (activeEndAt.Hour == 0 && activeEndAt.Minute == 0) {
					active += " - " + activeEndAt.ToString("d/M-yyyy");
				} else {
					active += " - " + activeEndAt.ToString("d/M-yyyy HH:mm");
				}
			}

			this.ActiveAtLabel.Text = active;
		}
	}
}