#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using com.podio;

#endregion

namespace dk.kirkeapp {
	public class Application {
		static void Main(string[] args) {
			UIApplication.Main(args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate {
		public static NSUserDefaults Defaults = NSUserDefaults.StandardUserDefaults;

		public static string FAVORITE_TAG = "favorit";

		private string _applicationName;

		public string ApplicationName {
			get {
				return _applicationName;
			}
		}

		private string _applicationUrl;

		public string ApplicationUrl {
			get {
				return _applicationUrl;
			}
		}

		private string _podioClientID;

		public string PodioClientID {
			get {
				return _podioClientID;
			}
		}

		private string _podioClientSecret;

		public string PodioClientSecret {
			get {
				return _podioClientSecret;
			}
		}

		private string _podioSpaceUrl;

		public string PodioSpaceUrl {
			get {
				return _podioSpaceUrl;
			}
		}

		private string _podioMessagesAppID;

		public string PodioMessagesAppID {
			get {
				return _podioMessagesAppID;
			}
		}

		private string _podioEventsAppID;

		public string PodioEventsAppID {
			get {
				return _podioEventsAppID;
			}
		}

		private string _podioPagesAppID;

		public string PodioPagesAppID {
			get {
				return _podioPagesAppID;
			}
		}


		private com.podio.Client _client;

		public com.podio.Client PodioClient {
			get {
				return _client;
			}
		}

		public Space ActiveSpace {
			get;
			set;
		}

		public Contact ActiveContact {
			get;
			set;
		}

		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
			// load information about currently configured app
			NSDictionary prefs = NSDictionary.FromFile(Path.Combine(NSBundle.MainBundle.BundlePath, "Settings/App.plist"));
			_applicationName = prefs.ValueForKey(new NSString("ApplicationName")).ToString();
			_applicationUrl = prefs.ValueForKey(new NSString("ApplicationUrl")).ToString();
			_podioClientID = prefs.ValueForKey(new NSString("PodioClientID")).ToString();
			_podioClientSecret = prefs.ValueForKey(new NSString("PodioClientSecret")).ToString();
			_podioSpaceUrl = prefs.ValueForKey(new NSString("PodioSpaceUrl")).ToString();
			_podioMessagesAppID = prefs.ValueForKey(new NSString("PodioMessagesAppID")).ToString();
			_podioEventsAppID = prefs.ValueForKey(new NSString("PodioEventsAppID")).ToString();
			_podioPagesAppID = prefs.ValueForKey(new NSString("PodioPagesAppID")).ToString();

			_client = new com.podio.Client(_podioClientID, _podioClientSecret);

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