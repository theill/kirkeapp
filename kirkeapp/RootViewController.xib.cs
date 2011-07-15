#region Using directives
using System;
using System.IO;
using System.Json;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;

using com.podio;

using dk.kirkeapp.data;

#endregion

namespace dk.kirkeapp {
	partial class RootViewController : BackgroundViewController {
		public RootViewController(IntPtr handle) : base (handle) {
		}

		[Export ("initWithCoder:")]
		public RootViewController(NSCoder coder) : base (coder) {
		}

		void LoadGroups() {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioGroupsAppID), (rsp) => {
				List<Group > groups = new List<Group>();
				foreach (JsonValue item in rsp["items"]) {
					Item podioItem = Item.FromJson(item);

					Group g = new Group {
						ID = podioItem.ItemID,
						Name = podioItem.Title,
						Contacts = new List<Contact>()
					};
					foreach (var field in podioItem.Fields) {
						if (field.ExternalID == "kontakt") {
							foreach (var contact in field.Values) {
								g.Contacts.Add(Contact.FromJson(contact.ObjectValue["value"] as JsonValue));
							}
						}
					}

					groups.Add(g);
				}
				appDelegate.Groups = groups;
			}, (err) => {
				Console.WriteLine("Failed to load groups");

			});
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			btnMessages.Enabled = false;
			btnDonation.Enabled = false;

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

			appDelegate.PodioClient.Authenticate(AppDelegate.PODIO_USERNAME, AppDelegate.PODIO_PASSWORD, (oauth_token) => {

				LoadGroups();

				appDelegate.PodioClient._get(string.Format("/space/url?url={0}", appDelegate.PodioSpaceUrl), (rsp) => {
					Console.WriteLine("Got info from space");
					appDelegate.ActiveSpace = Space.FromJson(rsp);

					Console.WriteLine("Got space id of {0} created at {1}", appDelegate.ActiveSpace.SpaceID, appDelegate.ActiveSpace.CreatedOn);

					int profileID = AppDelegate.Defaults.IntForKey("profile_id");
					if (profileID > 0) {
						appDelegate.PodioClient._get(string.Format("/contact/{0}/v2", profileID), (rsp2) => {
							Console.WriteLine("Got active contact: {0}", (rsp2 as JsonObject));

							appDelegate.ActiveContact = Contact.FromJson(rsp2);

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

			btnMessages.SetBackgroundImage(new UIImage(), UIControlState.Normal);
			btnMessages.SetBackgroundImage(new UIImage(), UIControlState.Normal);
			btnCalendar.SetBackgroundImage(new UIImage(), UIControlState.Normal);
			btnBible.SetBackgroundImage(new UIImage(), UIControlState.Normal);
			btnPsalms.SetBackgroundImage(new UIImage(), UIControlState.Normal);
			btnFavorites.SetBackgroundImage(new UIImage(), UIControlState.Normal);

			btnMessages.TouchUpInside += (sender, e) => {
				Console.WriteLine("Displaying messages");
				var c = new MessagesViewController();
				NavigationController.PushViewController(c, true);
			};

			btnCalendar.TouchUpInside += (sender, e) => {
				Console.WriteLine("Displaying calendar");
				var c = new CalendarViewController();
				NavigationController.PushViewController(c, true);
			};

			this.btnDonation.TouchUpInside += (sender, e) => {
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

		public override void ViewDidAppear(bool animated) {
			base.ViewDidAppear(animated);

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			bool contactLoggedIn = appDelegate.ActiveContact != null;
			btnMessages.Enabled = contactLoggedIn;
			btnDonation.Enabled = contactLoggedIn;
		}

		void AccountClick(object sender, EventArgs e) {
			Console.WriteLine("Log in");
			this.NavigationController.PushViewController(new LogOnViewController(), false);
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