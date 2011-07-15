#region Using directives
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

#endregion

namespace dk.kirkeapp {
	public class JsonDataListDelegate<T> : UITableViewDelegate where T : IJsonData {
		private static UIFont defaultRenderingFont = UIFont.FromName("Helvetica", 13f);
//		UIViewController _controller;
		IJsonDataSource<T> _appd;
		Action<T> _selected;

		public JsonDataListDelegate(UIViewController controller, IJsonDataSource<T> appd, Action<T> selected) {
//			_controller = controller;
			_appd = appd;
			_selected = selected;
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath) {
			Console.WriteLine("Asking for height of row {0} with {1} elements", indexPath.Row, _appd.ListCount);
			if (_appd.ListCount <= indexPath.Row) {
				Console.WriteLine("We actually do not have a row here");
				return 64f;
			}

			var e = _appd.JsonData[indexPath.Row];

			OptionDictionary options = e.ToOptions();
			if (options != null && options.ContainsKey("Content")) {
				string text = (string)options["Content"] ?? "";

				SizeF size = tableView.StringSize(text, defaultRenderingFont, new SizeF(300f, 640f), UILineBreakMode.WordWrap);
				return Math.Max(size.Height + 35f, 64f);
			} else {
				return 64f;
			}
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
			var e = _appd.JsonData[indexPath.Row];

			if (_selected != null) {
				_selected.Invoke(e);
			}
		}
	}
}