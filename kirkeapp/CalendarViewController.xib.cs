#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Json;

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

		public string CellNibName {
			get {
				return "EventCellViewController";
			}
		}
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			NavigationItem.Title = "Kalender";

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

			appDelegate.PodioClient._get(string.Format("/calendar/app/{0}/?app_id={0}&date_from={1}&date_to=2020-01-01&types=item", appDelegate.PodioEventsAppID, today), (rsp) => {
				JsonArray items = (JsonArray)rsp;

				_data = new List<Event>();
				foreach (JsonValue item in items) {
					Console.WriteLine("test3");
					_data.Add(new Event { Title = item["title"], ActiveAt = DateTime.Parse(item["start"]) });
					//				item["start"]
					//				item["end"]
					//				item["title"]
					//				item["id"]
				}
				//[{"start":"2011-06-16","group":"Event","org":{"type":"free","premium":false,"name":"Kirkeapp","logo":null,"url":"https:\/\/kirkeapp.podio.com\/","url_label":"kirkeapp","image":null,"org_id":20617},"title":"Pigekoret kommer og spiller","link":"https:\/\/kirkeapp.podio.com\/kokkedal\/item\/685323","end":"2011-06-16","app":{"item_name":"Event","url_label":"events","icon":"44.png","app_id":291877,"name":"Events"},"space":{"url":"https:\/\/kirkeapp.podio.com\/kokkedal\/","url_label":"kokkedal","space_id":55853,"name":"Kokkedal"},"type":"item","id":685323}]

				InvokeOnMainThread(() => {
					this.CalendarTableView.DataSource = new JsonDataSource<Event>(this);
					this.CalendarTableView.Delegate = new JsonDataListDelegate<Event>(this, this, (evt) => {
						Console.WriteLine("Event {0} has been selected", evt);
					});
					this.CalendarTableView.ReloadData();
				});
			}, (error) => {
				Console.WriteLine("Unable to read calendar events");
			});
		}
	}
}