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

			UIImage image = UIImage.FromBundle("Images/bg-normal.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.SendSubviewToBack(a);

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioPagesAppID), (rsp) => {
				JsonArray items = (JsonArray)rsp["items"];

				foreach (JsonValue item in items) {
					if (item["title"] == "Forside") {
						JsonArray fields = (JsonArray)item["fields"];
						foreach (JsonObject field in fields) {
//							 {"values": [{"value": "<p>Beskrivelse af Kokkedal kirke.</p>"}], "type": "text", "field_id": 2342768, "external_id": "beskrivelse", "label": "Beskrivelse"}

							if (field.AsString("label") == "Beskrivelse") {
								InvokeOnMainThread(() => {
									this.DescriptionLabel.Text = HtmlRemoval.StripTags(((JsonArray)field["values"])[0]["value"]);
								});
							} else if (field.AsString("label") == "Lokation") {
								InvokeOnMainThread(() => {
									this.Address1Label.Text = ((JsonArray)field["values"])[0]["value"];
									this.Address2Label.Text = "";
								});
							}
						}
					}
				}
			}, (error) => {
				Console.WriteLine("Unable to read info about church");
			});

			this.AddressMapView.Delegate = new MapViewDelegate(this);
		}
	}

	public class MapViewDelegate : MKMapViewDelegate {

//		private AboutViewController _appd;

		public MapViewDelegate(AboutViewController appd) : base() {
//			_appd = appd;
		}

		public override void DidUpdateUserLocation(MonoTouch.MapKit.MKMapView mapView, MonoTouch.MapKit.MKUserLocation userLocation) {
			Console.WriteLine("DidUpdateUserLocation");

//#if DEBUG
//				CLLocation location = new CLLocation(55.66657164, 12.55914527);
//#else
			CLLocation location = userLocation.Location;
//#endif
			mapView.SetCenterCoordinate(location.Coordinate, true);
			MKCoordinateSpan span = new MKCoordinateSpan(0.005, 0.005);
			MKCoordinateRegion region = new MKCoordinateRegion(location.Coordinate, span);
			mapView.SetRegion(region, true);
		}
	}
}