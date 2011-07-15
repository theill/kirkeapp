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
using System.Globalization;

#endregion

namespace dk.kirkeapp {
	public partial class AboutViewController : BackgroundViewController {
		public string Address {
			get {
				return Address1Label.Text;
			}
		}
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

			NavigationItem.Title = "Om";

			UIImage image = UIImage.FromBundle("Images/brown-gradient.png");
			UIImageView a = new UIImageView(image);
			View.AddSubview(a);

			Address1Label.Text = "";

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioPagesAppID), (rsp) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

				JsonArray items = (JsonArray)rsp["items"];

				string description = string.Empty, address = string.Empty;
				double latitude = 0, longitude = 0;
				foreach (JsonValue item in items) {
					if (item["title"] == "Forside") {
						JsonArray fields = (JsonArray)item["fields"];
						foreach (JsonObject field in fields) {
//							 {"values": [{"value": "<p>Beskrivelse af Kokkedal kirke.</p>"}], "type": "text", "field_id": 2342768, "external_id": "beskrivelse", "label": "Beskrivelse"}

							if (field.AsString("external_id") == "beskrivelse") {
								description = HtmlRemoval.StripTags(field["values"][0]["value"]);
							} else if (field.AsString("external_id") == "lokation") {
								address = field["values"][0]["value"];
							} else if (field.AsString("external_id") == "latitude") {
								latitude = Convert.ToDouble(field["values"][0].AsString("value"), appDelegate.EnglishFormatProvider);
							} else if (field.AsString("external_id") == "longitude") {
								longitude = Convert.ToDouble(field["values"][0].AsString("value"), appDelegate.EnglishFormatProvider);
							}
						}
					}
				}

				InvokeOnMainThread(() => {
					DescriptionTextView.Text = description;
					Address1Label.Text = address;

					AddressMapView.Delegate = new MapViewDelegate(appDelegate);

					if (latitude != 0.0 && longitude != 0.0) {
						CLLocationCoordinate2D coord = new CLLocationCoordinate2D(latitude, longitude);
						MKCoordinateSpan span = new MKCoordinateSpan(0.005, 0.005);
						MKCoordinateRegion region = new MKCoordinateRegion(coord, span);

						AddressMapView.SetCenterCoordinate(coord, true);
						AddressMapView.SetRegion(region, true);

						MyAnnotation annotation = new MyAnnotation(coord, appDelegate.ApplicationName, "Vis i fuldskærm");
						AddressMapView.AddAnnotationObject(annotation);
					}
				});

			}, (error) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				Console.WriteLine("Unable to read info about church");
			});
		}

//		public override void ViewDidAppear(bool animated) {
//			base.ViewDidAppear(animated);
//
//			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
//			AddressMapView.Delegate = new MapViewDelegate(appDelegate);
//		}
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
		public MyAnnotation(CLLocationCoordinate2D coord, string title, string subtitle) : base() {
			_coordinate = coord;
			_title = title;
			_subtitle = subtitle;
		}
	}


	/// <summary>
	///
	/// </summary>
	public class MapViewDelegate : MKMapViewDelegate {
		private AppDelegate _appd;

		public MapViewDelegate(AppDelegate appd):base() {
			_appd = appd;
		}

		public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, NSObject annotation) {
			Console.WriteLine("attempt to get view for MKAnnotation " + annotation);
			try {
				var anv = mapView.DequeueReusableAnnotation("thislocation");
				if (anv == null) {
					var pinanv = new MKPinAnnotationView(annotation, "thislocation");
					pinanv.AnimatesDrop = true;
					pinanv.PinColor = MKPinAnnotationColor.Red;
					pinanv.CanShowCallout = false;

					UIButton btn = UIButton.FromType(UIButtonType.DetailDisclosure);
					btn.TouchUpInside += (sender, e) => {
						Console.WriteLine("Going to start maps app");
//						Console.WriteLine("We have coord: {0} and {1}", mapView.CenterCoordinate.Latitude, mapView.CenterCoordinate.Longitude);
//						string url = string.Format("http://maps.google.com/maps?ll={0},{1}", mapView.CenterCoordinate.Latitude.ToString(_appd.EnglishFormatProvider), mapView.CenterCoordinate.Longitude.ToString(_appd.EnglishFormatProvider));
//						string url = string.Format("http://maps.google.com/maps?saddr={0}", _appd.Address);
//						Console.WriteLine("Got url {0}", url);
//						UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(url));
					};
					pinanv.RightCalloutAccessoryView = btn;
					anv = pinanv;
				} else {
					anv.Annotation = annotation;
				}
				return anv;
			} catch (Exception ex) {
				Console.WriteLine("GetViewForAnnotation Exception " + ex);
				return null;
			}
		}
	}
}