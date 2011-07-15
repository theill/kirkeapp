	#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using com.podio;

#endregion

namespace dk.kirkeapp {
	public partial class MessagesViewController : BackgroundViewController, IJsonDataSource<Message> {
		private List<Message> _data;

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

			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Ny", UIBarButtonItemStyle.Plain, (sender, e) => {
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

		private void LoadMessages() {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioMessagesAppID), (rsp) => {
				JsonArray items = (JsonArray)rsp["items"];

				this._data = new List<Message>();

				foreach (JsonObject item in items) {
					Message m = new Message();
					m.ID = item.AsInt32("item_id");
					m.Title = item.AsString("title");
					DateTime sentAt;
					DateTime.TryParse(item["initial_revision"].AsString("created_on"), out sentAt);
					m.SentAt = sentAt;

					int authorProfileID = 0, recipientProfileID = 0;

					JsonArray fields = (JsonArray)item["fields"];
					foreach (JsonObject field in fields) {
						string external_id = field.AsString("external_id");

						if (external_id == "body") {
							m.Content = HtmlRemoval.StripTags(field["values"][0].AsString("value"));
						} else if (external_id == "author") {
							m.From = field["values"][0]["value"].AsString("name");
							authorProfileID = field["values"][0]["value"].AsInt32("profile_id");
						} else if (external_id == "modtager") {
							m.To = field["values"][0]["value"].AsString("name");
							recipientProfileID = field["values"][0]["value"].AsInt32("profile_id");
						}
					}

					// set defaults
					if (!string.IsNullOrEmpty(m.Title) && !string.IsNullOrEmpty(m.Content)) {
						m.Content = m.Title + ": " + m.Content;
					} else if (!string.IsNullOrEmpty(m.Title)) {
						m.Content = m.Title;
					}

					if (string.IsNullOrEmpty(m.From)) {
						m.From = "Ukendt";
					}

					if (string.IsNullOrEmpty(m.To)) {
						m.To = "Ukendt";
					}

					if (authorProfileID == appDelegate.ActiveContact.ProfileID || recipientProfileID == appDelegate.ActiveContact.ProfileID) {
						_data.Add(m);
					} else {
						Console.WriteLine("Not gonna add {0} since profile [{1}, {2}] doesn't match logged on {3}", m.Title, authorProfileID, recipientProfileID, appDelegate.ActiveContact.ProfileID);
					}
				}

				InvokeOnMainThread(() => {
					this.tblMessages.DataSource = new JsonDataSource<Message>(this);
					this.tblMessages.ReloadData();
				});
			}, (error) => {
				Console.WriteLine("Unable to read messages");
			});
		}
	}
}

