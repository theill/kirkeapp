#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

#endregion

namespace dk.kirkeapp {
	public class JsonDataListDelegate<T> : UITableViewDelegate {
		UIViewController _controller;
		IJsonDataSource<T> _appd;

		public JsonDataListDelegate(UIViewController controller, IJsonDataSource<T> appd) {
			_controller = controller;
			_appd = appd;
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath) {
			return 64f;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
			int row = indexPath.Row;
			var e = _appd.JsonData[row];
//			string rowValue = e.From;
			Console.WriteLine("Selected {0}", e);

			// TODO: call event
		}
	}
}