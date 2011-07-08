#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class WebPageViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public WebPageViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public WebPageViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public WebPageViewController() : base ("WebPageViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public string Css {
			get;
			set;
		}

		public string Html {
			get;
			set;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			if (string.IsNullOrEmpty(this.Css)) {
				this.Css = "body, p { font-family: Helvetica; font-size: 12pt; }";
			}

//			this.WebView.Delegate = new WebPageViewDelegate(this);
			this.WebView.LoadHtmlString(string.Format("<html><head><style>{0}</style></head><body style='background-color:transparent'><div id='body'>{1}</div></body></html>", this.Css, this.Html), new NSUrl("http://kirkeapp.dk/"));
			this.WebView.BackgroundColor = UIColor.Clear;
			this.WebView.Opaque = false;
		}

//		public class WebPageViewDelegate : UIWebViewDelegate {
//			private WebPageViewController _controller;
//
//			public WebPageViewDelegate(WebPageViewController c) {
//				_controller = c;
//			}
//
//			public override void LoadingFinished(UIWebView webView) {
//				// TODO: Implement - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
//
//				string heightString = webView.EvaluateJavascript("document.getElementById(\"body\").offsetHeight;");
//				float h = Convert.ToSingle(heightString) + 12.0f;
//
//				webView.Frame = new System.Drawing.RectangleF(webView.Frame.X, webView.Frame.Y, webView.Frame.Size.Width, h);
//
////				h = webView.Frame.Y + h + 70;
//
//				_controller.ScrollView.ContentSize = new System.Drawing.SizeF(320, h);
//
//			}
//		}
	}
}