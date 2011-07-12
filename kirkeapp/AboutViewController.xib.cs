#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using System.Json;
using com.podio;

#endregion

namespace dk.kirkeapp {
	public partial class AboutViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public AboutViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public AboutViewController(NSCoder coder) : base (coder) {
			Initialize();
		}

		public AboutViewController() : base("AboutViewController", null) {
			Initialize();
		}

		void Initialize() {
		}

		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Om";

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioPagesAppID), (rsp) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

				JsonArray items = (JsonArray)rsp["items"];

				double latitude = 0, longitude = 0;
				foreach (JsonValue item in items) {
					if (item["title"] == "Forside") {
						JsonArray fields = (JsonArray)item["fields"];
						foreach (JsonObject field in fields) {
//							 {"values": [{"value": "<p>Beskrivelse af Kokkedal kirke.</p>"}], "type": "text", "field_id": 2342768, "external_id": "beskrivelse", "label": "Beskrivelse"}

							if (field.AsString("external_id") == "beskrivelse") {
								InvokeOnMainThread(() => {
									this.DescriptionTextView.Text = HtmlRemoval.StripTags(field["values"][0]["value"]);
								});
							} else if (field.AsString("external_id") == "lokation") {
								InvokeOnMainThread(() => {
									this.Address1Label.Text = field["values"][0]["value"];
								});
							} else if (field.AsString("external_id") == "latitude") {
								latitude = Convert.ToDouble(field["values"][0].AsString("value"));
							} else if (field.AsString("external_id") == "longitude") {
								longitude = Convert.ToDouble(field["values"][0].AsString("value"));
							}
						}
					}
				}

				CLLocation location = new CLLocation(latitude, longitude);
				MKCoordinateSpan span = new MKCoordinateSpan(0.005, 0.005);
				MKCoordinateRegion region = new MKCoordinateRegion(location.Coordinate, span);

				InvokeOnMainThread(() => {
					AddressMapView.SetCenterCoordinate(location.Coordinate, true);
					AddressMapView.SetRegion(region, true);

					MyAnnotation a = new MyAnnotation(location.Coordinate, "", "");
					AddressMapView.AddAnnotationObject(a);
				});

			}, (error) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				Console.WriteLine("Unable to read info about church");
			});
		}
	}

	/// <summary>
	/// MKAnnotation is an abstract class (in Objective C I think it's a protocol).
	/// Therefore we must create our own implementation of it. Since all the properties
	/// are read-only, we have to pass them in via a constructor.
	/// </summary>
	public class MyAnnotation : MKAnnotation {
		private CLLocationCoordinate2D _coordinate;
		private string _title, _subtitle;

		public override CLLocationCoordinate2D Coordinate {
			get {
				return _coordinate;
			}
			set {
				_coordinate = value;
			}
		}

		public override string Title {
			get {
				return _title;
			}
		}

		public override string Subtitle {
			get {
				return _subtitle;
			}
		}
		/// <summary>
		/// custom constructor
		/// </summary>
		public MyAnnotation(CLLocationCoordinate2D coord, string t, string s) : base() {
			_coordinate = coord;
			_title = t; 
			_subtitle = s;
		}
	}
}