#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public class JsonDataSource<T> : UITableViewDataSource where T : IJsonData {
		public static NSString kCellIdentifier = new NSString("CellIdentifier");
		private IJsonDataSource<T> _appd;
		private Dictionary<int, IJsonCellController> controllers = new Dictionary<int, IJsonCellController>();

		public JsonDataSource(IJsonDataSource<T> appd) {
			_appd = appd;
		}

		public override int RowsInSection(UITableView tableview, int section) {
			return _appd.ListCount;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath) {
			IJsonCellController cellController = null;

			var cell = tableView.DequeueReusableCell(JsonDataSource<T>.kCellIdentifier);
			if (cell == null) {
				// FIXME: figure out if you can do this dynamically (without reflection :))
				if (_appd.CellNibName == "MessageCellViewController") {
					cellController = new MessageCellViewController();
				} else if (_appd.CellNibName == "EventCellViewController") {
					cellController = new EventCellViewController();
				} else {
					cellController = new TitleCellViewController();
				}

				NSBundle.MainBundle.LoadNib(_appd.CellNibName, (NSObject)cellController, null);
				cell = cellController.ViewCell;
				cell.SelectedBackgroundView = new UIView(); // important to create it - otherwise you can't set color
				cell.SelectedBackgroundView.BackgroundColor = UIColor.FromRGB(235, 232, 217);

				cell.Tag = Environment.TickCount;
				controllers.Add(cell.Tag, cellController);
			} else {
				cellController = controllers[cell.Tag];
			}

			int row = indexPath.Row;
			var e = (IJsonData)_appd.JsonData[row];

			cellController.Configure(e.ToOptions());

			return cell;
		}
	}
}