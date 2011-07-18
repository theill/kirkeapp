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
	public partial class FavoritesViewController : BackgroundViewController, IJsonDataSource<Favorite> {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public FavoritesViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public FavoritesViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public FavoritesViewController() : base ("FavoritesViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		#region IJsonDataSource[Favorite] implementation
		private List<Favorite> _data;
		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<Favorite> JsonData {
			get {
				return _data;
			}
		}

		public string CellNibName {
			get {
				return "TitleCellViewController";
			}
		}
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Favoritter";

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			FavoritesTableView.SeparatorColor = UIColor.FromRGB(217, 212, 199);

			_data = new List<Favorite>();

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			appDelegate.PodioClient._get(string.Format("/item/app/{0}/?tags={1}", appDelegate.PodioPagesAppID, AppDelegate.FAVORITE_TAG), (rsp) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

				JsonArray items = (JsonArray)rsp["items"];

				foreach (JsonValue item in items) {
					#region Sample response
//{
//    "files": 0,
//    "ratings": {
//        "like": {
//            "counts": {}
//        }
//    },
//    "title": "Trosbekendelsen",
//    "fields": [{
//        "values": [{
//            "value": "Trosbekendelsen"
//        }],
//        "type": "title",
//        "field_id": 2342767,
//        "external_id": "sider",
//        "label": "Sider"
//    },
//    {
//        "values": [{
//            "value": "<blockquote><i>Vi forsager Djævelen og alle hans gerninger og alt hans væsen.</i></blockquote><blockquote><i>Vi tror på Gud Fader, den almægtige,</i><br /><i>himmelens og jordens skaber.</i></blockquote><blockquote><i>Vi tror på Jesus Kristus,</i><br /><i>hans enbårne Søn, vor Herre,</i><br /><i>som er undfanget ved Helligånden,</i><br /><i>født af Jomfru Maria,</i><br /><i>pint under Pontius Pilatus,</i><br /><i>korsfæstet, død og begravet,</i><br /><i>nedfaret til Dødsriget,</i><br /><i>på tredje dag opstanden fra de døde,</i><br /><i>opfaret til himmels,</i><br /><i>siddende ved Gud Faders, den almægtiges, højre hånd,</i><br /><i>hvorfra han skal komme at dømme levende og døde.</i></blockquote><blockquote><i>Vi tror på Helligånden,</i><br /><i>den hellige, almindelige kirke,</i><br /><i>de helliges samfund, syndernes forladelse,</i><br /><i>kødets opstandelse og det evige liv.</i><br /><i>Amen.</i></blockquote>"
//        }],
//        "type": "text",
//        "field_id": 2342768,
//        "external_id": "beskrivelse",
//        "label": "Beskrivelse"
//    }],
//    "initial_revision": {
//        "created_on": "2011-07-11 09:18:59",
//        "created_by": {
//            "user_id": 5183,
//            "avatar": 2596,
//            "name": "Peter Theill",
//            "url": "https://podio.com/-/contacts/5183",
//            "avatar_id": 2596,
//            "image": {
//                "link": "https://download.podio.com/2596",
//                "file_id": 2596
//            },
//            "type": "user",
//            "id": 5183,
//            "avatar_type": "file"
//        },
//        "created_via": {
//            "url": null,
//            "display": false,
//            "id": 1,
//            "name": "Podio Development"
//        },
//        "user": {
//            "user_id": 5183,
//            "avatar": 2596,
//            "name": "Peter Theill",
//            "url": "https://podio.com/-/contacts/5183",
//            "avatar_id": 2596,
//            "image": {
//                "link": "https://download.podio.com/2596",
//                "file_id": 2596
//            },
//            "type": "user",
//            "id": 5183,
//            "avatar_type": "file"
//        },
//        "type": "creation",
//        "revision": 0
//    },
//    "current_revision": {
//        "created_on": "2011-07-11 09:18:59",
//        "created_by": {
//            "user_id": 5183,
//            "avatar": 2596,
//            "name": "Peter Theill",
//            "url": "https://podio.com/-/contacts/5183",
//            "avatar_id": 2596,
//            "image": {
//                "link": "https://download.podio.com/2596",
//                "file_id": 2596
//            },
//            "type": "user",
//            "id": 5183,
//            "avatar_type": "file"
//        },
//        "created_via": {
//            "url": null,
//            "display": false,
//            "id": 1,
//            "name": "Podio Development"
//        },
//        "user": {
//            "user_id": 5183,
//            "avatar": 2596,
//            "name": "Peter Theill",
//            "url": "https://podio.com/-/contacts/5183",
//            "avatar_id": 2596,
//            "image": {
//                "link": "https://download.podio.com/2596",
//                "file_id": 2596
//            },
//            "type": "user",
//            "id": 5183,
//            "avatar_type": "file"
//        },
//        "type": "creation",
//        "revision": 0
//    },
//    "comments": 0,
//    "link": "https://kirkeapp.podio.com/kokkedal/item/1002881",
//    "item_id": 1002881,
//    "external_id": null
//}

					#endregion

					Favorite f = new Favorite {
						Title = item.AsString("title")
					};

					JsonArray fields = (JsonArray)item["fields"];
					foreach (JsonObject field in fields) {
						if (field.AsString("external_id") == "beskrivelse") {
							f.Content = field["values"][0]["value"];
						}
					}

					_data.Add(f);
				}

				InvokeOnMainThread(() => {
					this.FavoritesTableView.DataSource = new JsonDataSource<Favorite>(this);
					this.FavoritesTableView.ReloadData();
				});

			}, (error) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				Log.WriteLine("Unable to read pages");
			});

			this.FavoritesTableView.Delegate = new JsonDataListDelegate<Favorite>(this, this, (fav) => {
				Log.WriteLine("Favorite {0} has been selected", fav);

				NavigationController.PushViewController(new WebPageViewController {
					Title = fav.Title,
					Html = fav.Content
				}, true);
			});
		}
	}
}