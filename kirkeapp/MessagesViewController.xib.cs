#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using com.podio;

using dk.kirkeapp.data;
#endregion

namespace dk.kirkeapp {
	public partial class MessagesViewController : BackgroundViewController, IJsonDataSource<Message> {
		private List<Message> _data = new List<Message>();

		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<Message> JsonData {
			get {
				return _data;
			}
		}

		public string CellNibName {
			get {
				return "MessageCellViewController";
			}
		}

		#region Constructors

		// The IntPtr and initWithCoder constructors are required for items that need
		// to be able to be created from a xib rather than from managed code

		public MessagesViewController(IntPtr handle) : base (handle) {
			Initialize();
		}

		[Export ("initWithCoder:")]
		public MessagesViewController(NSCoder coder) : base (coder) {
			Initialize();
		}

		public MessagesViewController() : base ("MessagesViewController", null) {
			Initialize();
		}

		void Initialize() {
		}

		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			NavigationItem.Title = "Beskeder";

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			tblMessages.SeparatorColor = UIColor.FromRGB(217, 212, 199);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem("Ny", UIBarButtonItemStyle.Plain, (sender, e) => {
				var c = new NewMessageViewController();
				NavigationController.PushViewController(c, true);
			});

			tblMessages.Delegate = new JsonDataListDelegate<Message>(this, this, (msg) => {
				InvokeOnMainThread(() => {
					NavigationController.PushViewController(new MessageViewController { PrimaryMessage = msg }, true);
				});
			});

			LoadMessages();
		}

		private List<int> ExpandProfilesInGroup(int groupID) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			Group g = appDelegate.Groups.Find((a) => a.ID == groupID);
			return (g != null) ? g.Contacts.ConvertAll<int>((a) => {
				return a.ProfileID; }) : new List<int>();
		}

		private void LoadMessages() {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioMessagesAppID), (rsp) => {
				JsonArray items = (JsonArray)rsp["items"];

				_data = new List<Message>();

				foreach (JsonObject item in items) {
					Message m = Message.Parse(item);

					List<int > destinationProfiles = new List<int>();
					destinationProfiles.Add(m.AuthorProfileID);
					destinationProfiles.Add(m.RecipientProfileID);
					destinationProfiles.AddRange(ExpandProfilesInGroup(m.RecipientGroupID));
					Console.WriteLine("After group has been expanded we have: {0}", destinationProfiles);

					if (destinationProfiles.Contains(appDelegate.ActiveContact.ProfileID)) {
						_data.Add(m);
					} else {
						Console.WriteLine("Not gonna add {0} since profile [{1}, {2}] doesn't match logged on {3}", m.Title, m.AuthorProfileID, m.RecipientProfileID, appDelegate.ActiveContact.ProfileID);
					}
				}

				InvokeOnMainThread(() => {
					tblMessages.DataSource = new JsonDataSource<Message>(this);
					tblMessages.ReloadData();
				});
			}, (error) => {
				Console.WriteLine("Unable to read messages");
			});
		}
	}
}