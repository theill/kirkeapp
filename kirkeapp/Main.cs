#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;

#endregion

namespace dk.kirkeapp {
	public class Application {
		static void Main(string[] args) {
			UIApplication.Main(args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate {
		private string _applicationName;
		private string _applicationUrl;

		public string ApplicationName {
			get {
				return _applicationName;
			}
		}

		public string ApplicationUrl {
			get {
				return _applicationUrl;
			}
		}

		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
			// load information about currently configured app
			NSDictionary prefs = NSDictionary.FromFile(Path.Combine(NSBundle.MainBundle.BundlePath, "Settings/App.plist"));
			_applicationName = prefs.ValueForKey(new NSString("ApplicationName")).ToString();
			_applicationUrl = prefs.ValueForKey(new NSString("ApplicationUrl")).ToString();

			window.AddSubview(navigationController.View);
			window.MakeKeyAndVisible();
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated(UIApplication application) {
		}
		
		/*
		public override void WillTerminate (UIApplication application)
		{
			//Save data here
		}
		*/
	}
}

