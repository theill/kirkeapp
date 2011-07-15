#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

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

		public string ImageFilename {
			get;
			set;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			if (string.IsNullOrEmpty(this.Css)) {
				this.Css = @"body { background-color: rgb(250, 249, 235); margin: 0; padding: 6px; }
body, p { font-family: Helvetica; font-size: 12pt; }
a { color: rgb(141, 58, 5); }
//img.primary { -webkit-transform-style:preserve-3d; -webkit-transform: rotate(-2deg); }";
			}

			string imageHtml = string.Empty;
			if (!string.IsNullOrEmpty(ImageFilename)) {
				NSUrl imageUrl = NSUrl.FromFilename(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ImageFilename));
				Console.WriteLine("We have a file: {0}", imageUrl.ToString());
				imageHtml = string.Format("<img class='primary' src='{0}' width='300' />", imageUrl.ToString());
			}

//			this.WebView.Delegate = new WebPageViewDelegate(this);
			WebView.LoadHtmlString(string.Format("<html><head><style>{0}</style></head><body><div id='body'>{2}{1}</div></body></html>", this.Css, this.Html, imageHtml), null);
			WebView.BackgroundColor = UIColor.FromRGB(250, 249, 235);

			if (WebView.Subviews.Length > 0) {
				foreach (var shadowView in WebView.Subviews[0].Subviews) {
					shadowView.Hidden = true;
				}

				WebView.Subviews[0].Subviews.Last().Hidden = false;
			}
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