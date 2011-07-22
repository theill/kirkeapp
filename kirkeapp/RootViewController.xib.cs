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
			}, AppDelegate.GenericErrorHandling);
		}

		public void Authenticated(string oauthToken) {
			// read all groups to figure out what messages are available for a given profile
			LoadGroups();

			// initialize active space
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/space/url?url={0}", appDelegate.PodioSpaceUrl), (rsp) => {
				appDelegate.ActiveSpace = Space.FromJson(rsp);

				int profileID = AppDelegate.Defaults.IntForKey("profile_id");
				if (profileID > 0) {
					appDelegate.PodioClient._get(string.Format("/contact/{0}/v2", profileID), (rsp2) => {
						appDelegate.ActiveContact = Contact.FromJson(rsp2);

						InvokeOnMainThread(() => {
							bool contactLoggedIn = appDelegate.ActiveContact != null;
							btnMessages.Enabled = contactLoggedIn;
							btnDonation.Enabled = contactLoggedIn;
						});

					}, AppDelegate.GenericErrorHandling);
				}

//				// set church image
//				Log.WriteLine("We got {0}", appDelegate.ActiveSpace.ImageFileID);
//				if (appDelegate.ActiveSpace.ImageFileID > 0) {
//					appDelegate.GetFile(appDelegate.ActiveSpace.ImageFileID, (filename) => {
//						InvokeOnMainThread(() => {
//							FrontImage.Image = UIImage.FromFile(filename);
//						});
//					});
//				}

				InvokeOnMainThread(() => {
					NavigationItem.Title = TitleLabel.Text = appDelegate.ActiveSpace.Name;
				});
			}, AppDelegate.GenericErrorHandling);
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			btnMessages.Enabled = false;
			btnDonation.Enabled = false;

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			NavigationItem.Title = "Kirken";
			TitleLabel.Text = "Indlæser...";

			UIImage image = UIImage.FromBundle("Images/brown-gradient.png");
			UIImageView a = new UIImageView(image);
			View.AddSubview(a);

			NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Konto", UIBarButtonItemStyle.Plain, AccountClick);

//			NavigationController.NavigationBar.Alpha = 0.40f;
			NavigationController.NavigationBar.TintColor = UIColor.FromRGB(158, 80, 23);

			btnMessages.SetBackgroundImage(UIImage.FromBundle("Images/buttons/mail-icon.png"), UIControlState.Normal);
			btnCalendar.SetBackgroundImage(UIImage.FromBundle("Images/buttons/calendar-icon.png"), UIControlState.Normal);
			btnDonation.Hidden = true;
			btnBible.SetBackgroundImage(UIImage.FromBundle("Images/buttons/bible-icon.png"), UIControlState.Normal);
			btnPsalms.SetBackgroundImage(UIImage.FromBundle("Images/buttons/psalms-icon.png"), UIControlState.Normal);
			btnFavorites.SetBackgroundImage(UIImage.FromBundle("Images/buttons/favorites-icon.png"), UIControlState.Normal);

			btnMessages.SetBackgroundImage(UIImage.FromBundle("Images/buttons/mail-icon-selected.png"), UIControlState.Highlighted);
			btnCalendar.SetBackgroundImage(UIImage.FromBundle("Images/buttons/calendar-icon-selected.png"), UIControlState.Highlighted);
			btnBible.SetBackgroundImage(UIImage.FromBundle("Images/buttons/bible-icon-selected.png"), UIControlState.Highlighted);
			btnPsalms.SetBackgroundImage(UIImage.FromBundle("Images/buttons/psalms-icon-selected.png"), UIControlState.Highlighted);
			btnFavorites.SetBackgroundImage(UIImage.FromBundle("Images/buttons/favorites-icon-selected.png"), UIControlState.Highlighted);

//			btnMessages.SetTitle("", UIControlState.Normal);
//			btnCalendar.SetTitle("", UIControlState.Normal);
//			btnDonation.SetTitle("", UIControlState.Normal);
//			btnBible.SetTitle("", UIControlState.Normal);
//			btnPsalms.SetTitle("", UIControlState.Normal);
//			btnFavorites.SetTitle("", UIControlState.Normal);

			FrontImage.Image = UIImage.FromBundle("Images/churches/frontpage.jpg");

			btnMessages.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new MessagesViewController(), true);
			};

			btnCalendar.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new CalendarViewController(), true);
			};

			this.btnDonation.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new DonationsViewController(), true);
			};

			this.btnAbout.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new AboutViewController(), true);
			};

			this.btnBible.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new BibleViewController(), true);
			};

			this.btnPsalms.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new PsalmsViewController(), true);
			};

			this.btnFavorites.TouchUpInside += (sender, e) => {
				NavigationController.PushViewController(new FavoritesViewController(), true);
			};

//			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
//			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

			appDelegate.PodioClient.Authenticate(AppDelegate.PODIO_USERNAME, AppDelegate.PODIO_PASSWORD, Authenticated, AppDelegate.GenericErrorHandling);
		}

		public override void ViewDidAppear(bool animated) {
			base.ViewDidAppear(animated);

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			bool contactLoggedIn = appDelegate.ActiveContact != null;
			btnMessages.Enabled = contactLoggedIn;
			btnDonation.Enabled = contactLoggedIn;
		}

		void AccountClick(object sender, EventArgs e) {
			NavigationController.PushViewController(new LogOnViewController(), false);
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