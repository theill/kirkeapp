#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public class JsonDataSource<IJsonData> : UITableViewDataSource {
		public static NSString kCellIdentifier = new NSString("CellIdentifier");
		private IJsonDataSource<IJsonData> _appd;

		public JsonDataSource(IJsonDataSource<IJsonData> appd) {
			_appd = appd;
		}

		public override int RowsInSection(UITableView tableview, int section) {
			return _appd.ListCount;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
			var cellController = new MessageCellViewController();

			var cell = tableView.DequeueReusableCell(JsonDataSource<IJsonData>.kCellIdentifier);
			if (cell == null) {
				NSBundle.MainBundle.LoadNib("MessageCellViewController", cellController, null);
				cell = cellController.ViewCell;

				cell.Tag = Environment.TickCount;
//				controllers.Add(cell.Tag, cellController);

//				cell = new UITableViewCell(UITableViewCellStyle.Default, JsonDataSource<T>.kCellIdentifier);
				//cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

			int row = indexPath.Row;
			var e = _appd.JsonData[row];
			e.ToOptions();

			Dictionary<string, string > options = new Dictionary<string, string>();
			options.Add("subject", "test subject");
			options.Add("content", "my content");
			options.Add("sent_at", "today");

			cellController.Configure(e.ToOptions());

//			cell.TextLabel.Text = e.ToString();
			return cell;
		}
	}
}

