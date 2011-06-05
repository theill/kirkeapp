#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

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

			UIImage image = UIImage.FromBundle("Images/paper-light.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.SendSubviewToBack(a);

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