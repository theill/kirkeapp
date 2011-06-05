#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

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

			this._data = new List<Message> { new Message { From = "Peter Theill", Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", SentAt = DateTime.Now },
				new Message { From = "Paulus Mikael Ekstrand", Content = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", SentAt = DateTime.Now } };

			this.tblMessages.DataSource = new JsonDataSource<Message>(this);
			this.tblMessages.Delegate = new JsonDataListDelegate<Message>(this, this);
		}
	}
}

