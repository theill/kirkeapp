#region Using directives
using System;
using System.IO;
using System.Json;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using com.podio;

#endregion

namespace dk.kirkeapp {
	partial class RootViewController : BackgroundViewController {
		public RootViewController(IntPtr handle) : base (handle) {
		}

		[Export ("initWithCoder:")]
		public RootViewController(NSCoder coder) : base (coder) {
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			appDelegate.PodioClient.authenticate_with_credentials("admin@kirkeapp.dk", "belle0", (oauth_token) => {
				appDelegate.PodioClient._get(string.Format("/space/url?url={0}", appDelegate.PodioSpaceUrl), (rsp) => {
					Console.WriteLine("Got info from space");
					appDelegate.ActiveSpace = Space.Parse(rsp);

					Console.WriteLine("Got space id of {0} created at {1}", appDelegate.ActiveSpace.SpaceID, appDelegate.ActiveSpace.CreatedOn);

					int profileID = AppDelegate.Defaults.IntForKey("profile_id");
					if (profileID > 0) {
						appDelegate.PodioClient._get(string.Format("/contact/{0}/v2", profileID), (rsp2) => {
							Console.WriteLine("Got active contact: {0}", (rsp2 as JsonObject));

							appDelegate.ActiveContact = Contact.Parse(rsp2);

							InvokeOnMainThread(() => {
								bool contactLoggedIn = appDelegate.ActiveContact != null;
								btnMessages.Enabled = contactLoggedIn;
								btnDonation.Enabled = contactLoggedIn;
							});

						}, (err) => {
							Console.WriteLine("Desvaerre, det gik ikke .. fik {0}", err);

			// FIXME: show error for user
						});
					}

					InvokeOnMainThread(() => {
						this.NavigationItem.Title = appDelegate.ActiveSpace.Name;
					});
				}, (err) => {
					Console.WriteLine("Failed to read info for space");

				});

			}, (err) => {
				Console.WriteLine("Failed to authenticate!");
			});

//			JsonValue item;
//
//			item = appDelegate.PodioClient._get("/item/685323");
//			Console.WriteLine("Got item " + item.ToString());
//
////			item = client._get(string.Format("/space/url?info=false&url={0}", System.Web.HttpUtility.UrlEncode("https://kirkeapp.podio.com/kokkedal/")));
////			// {"post_on_new_member":true,"url_label":"kokkedal","created_on":"2011-06-13 05:51:54","org":{"type":"free","premium":false,"name":"Kirkeapp","logo":null,"url":"https:\/\/kirkeapp.podio.com\/","url_label":"kirkeapp","image":null,"org_id":20617},"subscribed":1,"name":"Kokkedal","post_on_new_app":true,"rights":["add_app","add_contact","add_status","add_task","add_widget","subscribe","view"],"url":"https:\/\/kirkeapp.podio.com\/kokkedal\/","space_id":55853,"org_id":20617,"created_by":{"link":"https:\/\/podio.com\/-\/contacts\/5183","avatar":2596,"user_id":5183,"image":{"link":"https:\/\/download.podio.com\/2596","file_id":2596},"profile_id":5183,"type":"user","name":"Peter Theill"},"role":"regular"}
////			Console.WriteLine("Got space " + item);
//
//			item = appDelegate.PodioClient._get("/calendar/app/291877/?app_id=291877&date_from=2010-01-01&date_to=2020-01-01&types=item");
////[{"start":"2011-06-16","group":"Event","org":{"type":"free","premium":false,"name":"Kirkeapp","logo":null,"url":"https:\/\/kirkeapp.podio.com\/","url_label":"kirkeapp","image":null,"org_id":20617},"title":"Pigekoret kommer og spiller","link":"https:\/\/kirkeapp.podio.com\/kokkedal\/item\/685323","end":"2011-06-16","app":{"item_name":"Event","url_label":"events","icon":"44.png","app_id":291877,"name":"Events"},"space":{"url":"https:\/\/kirkeapp.podio.com\/kokkedal\/","url_label":"kokkedal","space_id":55853,"name":"Kokkedal"},"type":"item","id":685323}]
//			Console.WriteLine("Got items " + item.ToString());

			NavigationItem.Title = appDelegate.ApplicationName;

			UIImage image = UIImage.FromBundle("Images/brown-gradient.png");
			UIImageView a = new UIImageView(image);
			View.AddSubview(a);

			NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Konto", UIBarButtonItemStyle.Plain, AccountClick);

//			NavigationController.NavigationBar.Alpha = 0.40f;
//			NavigationController.NavigationBar.TintColor = UIColor.FromRGBA(128, 128, 128, 64);
			NavigationController.NavigationBar.TintColor = UIColor.FromRGB(158, 80, 23);

			btnMessages.SetBackgroundImage(UIImage.FromBundle("Images/mail_open.png"), UIControlState.Normal);
			btnCalendar.SetBackgroundImage(UIImage.FromBundle("Images/calendar.png"), UIControlState.Normal);
			btnDonation.Hidden = true;
			btnBible.SetBackgroundImage(UIImage.FromBundle("Images/bookmark.png"), UIControlState.Normal);
			btnPsalms.SetBackgroundImage(UIImage.FromBundle("Images/music.png"), UIControlState.Normal);
			btnFavorites.SetBackgroundImage(UIImage.FromBundle("Images/favorite.png"), UIControlState.Normal);
			btnMessages.SetTitle("", UIControlState.Normal);
			btnCalendar.SetTitle("", UIControlState.Normal);
			btnDonation.SetTitle("", UIControlState.Normal);
			btnBible.SetTitle("", UIControlState.Normal);
			btnPsalms.SetTitle("", UIControlState.Normal);
			btnFavorites.SetTitle("", UIControlState.Normal);

			btnMessages.TouchUpInside += delegate(object sender, EventArgs e) {
				Console.WriteLine("Displaying messages");
				var c = new MessagesViewController();
				NavigationController.PushViewController(c, true);
			};

			btnCalendar.TouchUpInside += delegate(object sender, EventArgs e) {
				Console.WriteLine("Displaying calendar");
				var c = new CalendarViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnDonation.TouchUpInside += delegate(object sender, EventArgs e) {
				Console.WriteLine("Displaying Donations");
				var c = new DonationsViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnAbout.TouchUpInside += (sender, e) => {
				Console.WriteLine("Displaying about");
				var c = new AboutViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnBible.TouchUpInside += (sender, e) => {
				Console.WriteLine("Displaying Bible");
				var c = new BibleViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnPsalms.TouchUpInside += (sender, e) => {
				Console.WriteLine("Displaying psalms");
				var c = new PsalmsViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnFavorites.TouchUpInside += (sender, e) => {
				Console.WriteLine("Displaying favorites");
				var c = new FavoritesViewController();
				NavigationController.PushViewController(c, true);
			};
		}

		void AccountClick(object sender, EventArgs e) {
			Console.WriteLine("Log in");
			var c = new LogOnViewController();
			this.NavigationController.PushViewController(c, false);
		}

		public override void DidReceiveMemoryWarning() {
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidUnload() {
			// Release anything that can be recreated in viewDidLoad or on demand.
			// e.g. this.myOutlet = null;
			
			base.ViewDidUnload();
		}
		
	}
	
}

