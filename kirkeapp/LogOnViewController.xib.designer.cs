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
	[MonoTouch.Foundation.Register("LogOnViewController")]
	public partial class LogOnViewController {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.UIKit.UITextField __mt_EmailTextField;
		
		private MonoTouch.UIKit.UITextField __mt_PasswordTextField;
		
		private MonoTouch.UIKit.UILabel __mt_DescriptionLabel;
		
		private MonoTouch.UIKit.UILabel __mt_UserNotFoundLabel;
		
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
		
		[MonoTouch.Foundation.Connect("EmailTextField")]
		private MonoTouch.UIKit.UITextField EmailTextField {
			get {
				this.__mt_EmailTextField = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("EmailTextField")));
				return this.__mt_EmailTextField;
			}
			set {
				this.__mt_EmailTextField = value;
				this.SetNativeField("EmailTextField", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("PasswordTextField")]
		private MonoTouch.UIKit.UITextField PasswordTextField {
			get {
				this.__mt_PasswordTextField = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("PasswordTextField")));
				return this.__mt_PasswordTextField;
			}
			set {
				this.__mt_PasswordTextField = value;
				this.SetNativeField("PasswordTextField", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("DescriptionLabel")]
		private MonoTouch.UIKit.UILabel DescriptionLabel {
			get {
				this.__mt_DescriptionLabel = ((MonoTouch.UIKit.UILabel)(this.GetNativeField("DescriptionLabel")));
				return this.__mt_DescriptionLabel;
			}
			set {
				this.__mt_DescriptionLabel = value;
				this.SetNativeField("DescriptionLabel", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("UserNotFoundLabel")]
		private MonoTouch.UIKit.UILabel UserNotFoundLabel {
			get {
				this.__mt_UserNotFoundLabel = ((MonoTouch.UIKit.UILabel)(this.GetNativeField("UserNotFoundLabel")));
				return this.__mt_UserNotFoundLabel;
			}
			set {
				this.__mt_UserNotFoundLabel = value;
				this.SetNativeField("UserNotFoundLabel", value);
			}
		}
	}
}
