// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace dk.kirkeapp {
	
	
	// Base type probably should be MonoTouch.UIKit.UIViewController or subclass
	[MonoTouch.Foundation.Register("AboutViewController")]
	public partial class AboutViewController {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.MapKit.MKMapView __mt_AddressMapView;
		
		private MonoTouch.UIKit.UILabel __mt_Address1Label;
		
		private MonoTouch.UIKit.UITextView __mt_DescriptionTextView;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("view")]
		private MonoTouch.UIKit.UIView view {
			get {
				this.__mt_view = ((MonoTouch.UIKit.UIView)(this.GetNativeField("view")));
				return this.__mt_view;
			}
			set {
				this.__mt_view = value;
				this.SetNativeField("view", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("AddressMapView")]
		private MonoTouch.MapKit.MKMapView AddressMapView {
			get {
				this.__mt_AddressMapView = ((MonoTouch.MapKit.MKMapView)(this.GetNativeField("AddressMapView")));
				return this.__mt_AddressMapView;
			}
			set {
				this.__mt_AddressMapView = value;
				this.SetNativeField("AddressMapView", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("Address1Label")]
		private MonoTouch.UIKit.UILabel Address1Label {
			get {
				this.__mt_Address1Label = ((MonoTouch.UIKit.UILabel)(this.GetNativeField("Address1Label")));
				return this.__mt_Address1Label;
			}
			set {
				this.__mt_Address1Label = value;
				this.SetNativeField("Address1Label", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("DescriptionTextView")]
		private MonoTouch.UIKit.UITextView DescriptionTextView {
			get {
				this.__mt_DescriptionTextView = ((MonoTouch.UIKit.UITextView)(this.GetNativeField("DescriptionTextView")));
				return this.__mt_DescriptionTextView;
			}
			set {
				this.__mt_DescriptionTextView = value;
				this.SetNativeField("DescriptionTextView", value);
			}
		}
	}
}
