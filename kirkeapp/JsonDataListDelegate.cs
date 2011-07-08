#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

#endregion

namespace dk.kirkeapp {
	public class JsonDataListDelegate<T> : UITableViewDelegate {
//		UIViewController _controller;
		IJsonDataSource<T> _appd;
		Action<T> _selected;

		public JsonDataListDelegate(UIViewController controller, IJsonDataSource<T> appd, Action<T> selected) {
//			_controller = controller;
			_appd = appd;
			_selected = selected;
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath) {
			return 64f;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
			var e = _appd.JsonData[indexPath.Row];

			if (_selected != null) {
				_selected.Invoke(e);
			}
		}
	}
}