#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class CalendarViewController : BackgroundViewController, IJsonDataSource<Event> {
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for items that need
		// to be able to be created from a xib rather than from managed code

		public CalendarViewController(IntPtr handle) : base (handle) {
			Initialize();
		}

		[Export ("initWithCoder:")]
		public CalendarViewController(NSCoder coder) : base (coder) {
			Initialize();
		}

		public CalendarViewController() : base ("CalendarViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		#endregion

		#region IJsonDataSource[Event] implementation
		List<Event> _data;
		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<Event> JsonData {
			get {
				return _data;
			}
		}
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			NavigationItem.Title = "Kalender";

			_data = new List<Event>() { new Event { Title = "Sommerfest" } };

			this.CalendarTableView.DataSource = new JsonDataSource<Event>(this);
			this.CalendarTableView.Delegate = new JsonDataListDelegate<Event>(this, this);
		}
	}
}

