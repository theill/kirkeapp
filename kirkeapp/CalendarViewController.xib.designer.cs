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
	[MonoTouch.Foundation.Register("CalendarViewController")]
	public partial class CalendarViewController {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.UIKit.UITableView __mt_CalendarTableView;
		
		private MonoTouch.UIKit.UILabel __mt_EmptyLabel;
		
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
		
		[MonoTouch.Foundation.Connect("CalendarTableView")]
		private MonoTouch.UIKit.UITableView CalendarTableView {
			get {
				this.__mt_CalendarTableView = ((MonoTouch.UIKit.UITableView)(this.GetNativeField("CalendarTableView")));
				return this.__mt_CalendarTableView;
			}
			set {
				this.__mt_CalendarTableView = value;
				this.SetNativeField("CalendarTableView", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("EmptyLabel")]
		private MonoTouch.UIKit.UILabel EmptyLabel {
			get {
				this.__mt_EmptyLabel = ((MonoTouch.UIKit.UILabel)(this.GetNativeField("EmptyLabel")));
				return this.__mt_EmptyLabel;
			}
			set {
				this.__mt_EmptyLabel = value;
				this.SetNativeField("EmptyLabel", value);
			}
		}
	}
}
