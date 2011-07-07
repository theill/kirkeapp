#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using com.podio;
using System.Json;

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

			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Ny", UIBarButtonItemStyle.Plain, (sender, e) => {
				var c = new NewMessageViewController();
				NavigationController.PushViewController(c, true);
			});

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/", appDelegate.PodioMessagesAppID), (rsp) => {
				JsonArray items = (JsonArray)rsp["items"];

				this._data = new List<Message>();

				foreach (JsonObject item in items) {
					Message m = new Message();
					m.Content = item.AsString("title");
					DateTime sentAt;
					DateTime.TryParse(item["initial_revision"].AsString("created_on"), out sentAt);
					m.SentAt = sentAt;

					JsonArray fields = (JsonArray)item["fields"];
					foreach (JsonObject field in fields) {
						string external_id = field.AsString("external_id");

						if (external_id == "title") {
							m.Title = field["values"][0]["value"];
						} else if (external_id == "body") {
							m.Content = HtmlRemoval.StripTags(field["values"][0]["value"]);
						} else if (external_id == "author") {
							m.From = field["values"][0]["value"]["name"];
						} else if (external_id == "modtager") {
							m.To = field["values"][0]["value"]["name"];
						}

						#region Example of returned fields
//"fields": [
//        {
//          "values": [
//            {
//              "value": "Jeg har syndet"
//            }
//          ],
//          "type": "title",
//          "field_id": 1981078,
//          "external_id": "title",
//          "label": "Emne"
//        },
//        {
//          "values": [
//            {
//              "value": "<p>jeg ved det var forkert men jeg spiste en STOR is.</p>"
//            }
//          ],
//          "type": "text",
//          "field_id": 1981079,
//          "external_id": "body",
//          "label": "Indhold"
//        },
//        {
//          "values": [
//            {
//              "value": {
//                "about": "Notes: ConquerWare CVR: 21288497\nAnniversary: 8/25/1977\nBirthday: 8/25/1977\nNotes: ConquerWare CVR: 21288497 \n\n\n\nWeb Page: http://www.theill.com/\nAnniversary: 8/25/1977\nBirthday: 8/25/1977\nNotes: ConquerWare CVR: 21288497 \n\n\n\n\n\n\n\n\n\nWeb Page: http://www.theill.com/",
//                "name": "Peter Theill",
//                "title": [
//                  "Founder"
//                ],
//                "external_id": "137459",
//                "space_id": 55853,
//                "profile_id": 2503297,
//                "mail": [
//                  "peter@theill.com",
//                  "theill@gmail.com",
//                  "peter@commanigy.com"
//                ],
//                "phone": [
//                  "+45 61715096",
//                  "+13045843455",
//                  "+14561715096",
//                  "+1 (304) 584-3455"
//                ],
//                "link": null,
//                "avatar": 722164,
//                "address": [
//                  "Sundholmsvej 49\n4TV\n2300 Copenhagen S\nDenmark",
//                  "Frederiksholms Kanal 4\n3TV\n1220 Copenhagen K\nDenmark"
//                ],
//                "organization": "Commanigy",
//                "type": "space",
//                "image": {
//                  "link": "https://download.podio.com/722164",
//                  "file_id": 722164
//                }
//              }
//            }
//          ],
//          "type": "contact",
//          "field_id": 1981083,
//          "external_id": "author",
//          "label": "Afsender"
//        },
//        {
//          "values": [
//            {
//              "value": {
//                "user_id": 65964,
//                "name": "Kirkeapp Client Admin",
//                "external_id": null,
//                "image": null,
//                "profile_id": 2243654,
//                "link": "https://podio.com/-/contacts/65964",
//                "mail": [
//                  "admin@kirkeapp.dk"
//                ],
//                "type": "user"
//              }
//            }
//          ],
//          "type": "contact",
//          "field_id": 2367459,
//          "external_id": "modtager",
//          "label": "Modtager"
//        }
						#endregion
					}

					_data.Add(m);
				}

				InvokeOnMainThread(() => {
					this.tblMessages.DataSource = new JsonDataSource<Message>(this);
					this.tblMessages.ReloadData();
				});
			}, (error) => {
				Console.WriteLine("Unable to read messages");
			});

			this.tblMessages.Delegate = new JsonDataListDelegate<Message>(this, this, (msg) => {
				Console.WriteLine("Message {0} has been selected", msg);

				var c = new MessageViewController {
					PrimaryMessage = msg
				};

				InvokeOnMainThread(() => {
					NavigationController.PushViewController(c, true);
				});
			});
		}
	}
}

