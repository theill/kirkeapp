#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using com.podio;

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

			CalendarTableView.SeparatorColor = UIColor.FromRGB(217, 212, 199);

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			appDelegate.PodioClient._get(string.Format("/calendar/app/{0}/?app_id={0}&date_from={1}&date_to=2020-01-01&types=item", appDelegate.PodioEventsAppID, today), (rsp) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

				//[{"start":"2011-06-16","group":"Event","org":{"type":"free","premium":false,"name":"Kirkeapp","logo":null,"url":"https:\/\/kirkeapp.podio.com\/","url_label":"kirkeapp","image":null,"org_id":20617},"title":"Pigekoret kommer og spiller","link":"https:\/\/kirkeapp.podio.com\/kokkedal\/item\/685323","end":"2011-06-16","app":{"item_name":"Event","url_label":"events","icon":"44.png","app_id":291877,"name":"Events"},"space":{"url":"https:\/\/kirkeapp.podio.com\/kokkedal\/","url_label":"kokkedal","space_id":55853,"name":"Kokkedal"},"type":"item","id":685323}]
				JsonArray items = (JsonArray)rsp;

				_data = new List<Event>();
				foreach (JsonValue item in items) {
					_data.Add(new Event { ID = item["id"], Title = item["title"], ActiveStartAt = item.AsDateTime("start"), ActiveEndAt = item.AsDateTime("end") });
				}

				if (_data.Count == 0) {
					// no upcoming events in calendar
					InvokeOnMainThread(() => {
						CalendarTableView.Hidden = true;
						EmptyLabel.Hidden = false;
					});
				}

				this.CalendarTableView.Delegate = new JsonDataListDelegate<Event>(this, this, (evt) => {
					Console.WriteLine("Event {0} has been selected", evt);

					string description = string.Empty;
					int file_id = 0;

					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					appDelegate.PodioClient._get(string.Format("/item/{0}/value", evt.ID), (rsp2) => {
						Console.WriteLine("Got {0}", rsp2);
						UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

						JsonArray values = (JsonArray)rsp2;
						foreach (var v in values) {
							Console.WriteLine("Looking at {0}", v);
							if (v["external_id"] == "description") {
								description = v["values"][0]["value"];
							} else if (v["external_id"] == "image") {
								file_id = v["values"][0]["value"].AsInt32("file_id");
							}
						}

						Console.WriteLine("Got file: {0}", file_id);

						InvokeOnMainThread(() => {
							NavigationController.PushViewController(new WebPageViewController {
								Title = evt.Title,
								Html = description
							}, true);
						});
					}, (error) => {
						Console.WriteLine("Failed to get item");
						UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					});
				});

				InvokeOnMainThread(() => {
					this.CalendarTableView.DataSource = new JsonDataSource<Event>(this);
					this.CalendarTableView.ReloadData();
				});
			}, (error) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				Console.WriteLine("Unable to read calendar events");
			});
		}
	}
}