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

			this._data = new List<Message> { new Message { From = "Peter Theill", Content = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", SentAt = DateTime.Now },
				new Message { From = "Paulus Mikael Ekstrand", Content = "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.", SentAt = DateTime.Now } };

			this.tblMessages.DataSource = new JsonDataSource<Message>(this);
			this.tblMessages.Delegate = new JsonDataListDelegate<Message>(this, this);
		}
	}

//	public class MessagesDataSource : UITableViewDataSource {
//		public static NSString kCellIdentifier = new NSString("CellIdentifier");
//		private IJsonDataSource<Message> _appd;
//
//		public MessagesDataSource(IJsonDataSource<Message> appd) {
//			_appd = appd;
//		}
//
//		public override int RowsInSection(UITableView tableview, int section) {
//			return _appd.ListCount;
//		}
//
//		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
//			var cell = tableView.DequeueReusableCell(MessagesDataSource.kCellIdentifier);
//			if (cell == null) {
//				cell = new UITableViewCell(UITableViewCellStyle.Default, MessagesDataSource.kCellIdentifier);
//				//cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//			}
//
//			int row = indexPath.Row;
//			var e = _appd.JsonData[row];
//			cell.TextLabel.Text = e.From;
//			return cell;
//		}
//	}
//
//	public class MessagesListDelegate : UITableViewDelegate {
//		UIViewController _controller;
//		IJsonDataSource<Message> _appd;
//
//		public MessagesListDelegate(UIViewController controller, IJsonDataSource<Message> appd) {
//			_controller = controller;
//			_appd = appd;
//		}
//
//		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
//			int row = indexPath.Row;
//			var e = _appd.JsonData[row];
//			string rowValue = e.From;
//			Console.WriteLine("selected " + rowValue);
//
//			var c = new MessageViewController();
//			c.Conversation = new Conversation() { Messages = new List<Message> { new Message { From = e.From } } };
//			_controller.NavigationController.PushViewController(c, true);
//		}
//	}
}

