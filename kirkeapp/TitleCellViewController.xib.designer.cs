// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace dk.kirkeapp {
	
	
	// Base type probably should be MonoTouch.UIKit.UIViewController or subclass
	[MonoTouch.Foundation.Register("TitleCellViewController")]
	public partial class TitleCellViewController {
		
		private MonoTouch.UIKit.UITableViewCell __mt_MainCell;
		
		private MonoTouch.UIKit.UILabel __mt_TitleLabel;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("MainCell")]
		private MonoTouch.UIKit.UITableViewCell MainCell {
			get {
				this.__mt_MainCell = ((MonoTouch.UIKit.UITableViewCell)(this.GetNativeField("MainCell")));
				return this.__mt_MainCell;
			}
			set {
				this.__mt_MainCell = value;
				this.SetNativeField("MainCell", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("TitleLabel")]
		private MonoTouch.UIKit.UILabel TitleLabel {
			get {
				this.__mt_TitleLabel = ((MonoTouch.UIKit.UILabel)(this.GetNativeField("TitleLabel")));
				return this.__mt_TitleLabel;
			}
			set {
				this.__mt_TitleLabel = value;
				this.SetNativeField("TitleLabel", value);
			}
		}
	}
}
