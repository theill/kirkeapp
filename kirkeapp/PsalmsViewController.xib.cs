#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public class FilteringDataSource : UITableViewSource {
		public static NSString kCellIdentifier = new NSString("CellIdentifier");
		private List<CellData> _data = new List<CellData>();
		private Dictionary<int, IJsonCellController> controllers = new Dictionary<int, IJsonCellController>();
		private Action<CellData> _selected;

		public FilteringDataSource() {
		}

		public FilteringDataSource(string filter, Action<CellData> selected) {
			_selected = selected;

			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				if (!string.IsNullOrEmpty(filter)) {
					_data = db.Query<CellData>("SELECT id AS ID, title AS Title FROM psalms WHERE no = ? OR title LIKE ? ORDER BY no", filter, "%" + filter + "%");
				} else {
					_data = new List<CellData>();
				}
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath) {
			IJsonCellController cellController = null;

			var cell = tableView.DequeueReusableCell(JsonDataSource<CellData>.kCellIdentifier);
			if (cell == null) {
				cellController = new TitleCellViewController();

				NSBundle.MainBundle.LoadNib("TitleCellViewController", (NSObject)cellController, null);
				cell = cellController.ViewCell;
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
				cell.SelectedBackgroundView = new UIView(); // important to create it - otherwise you can't set color
				cell.SelectedBackgroundView.BackgroundColor = UIColor.FromRGB(235, 232, 217);

				cell.Tag = Environment.TickCount;
				controllers.Add(cell.Tag, cellController);
			} else {
				cellController = controllers[cell.Tag];
			}

			int row = indexPath.Row;
			var e = (IJsonData)_data[row];

			cellController.Configure(e.ToOptions());

			return cell;
		}

		public override int RowsInSection(UITableView tableview, int section) {
			return _data.Count;
		}

		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath) {
			return 64f;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) {
			Console.WriteLine("Selected {0}", indexPath.Row);
			var e = _data[indexPath.Row];

			if (_selected != null) {
				_selected.Invoke(e);
			}
		}
	}

	public class PsalmsSearch : UISearchBarDelegate {
		private PsalmsViewController _controller;
		private UITableView _tableView;

		public PsalmsSearch(PsalmsViewController c, UITableView x) {
			_controller = c;
			_tableView = x;
		}

		public override bool ShouldBeginEditing(UISearchBar searchBar) {
			Console.WriteLine("Should BEGIN editing");
			_tableView.Tag = 1;
			return true;
		}

		public override bool ShouldEndEditing(UISearchBar searchBar) {
			Console.WriteLine("Should END editing");
			_tableView.Tag = 0;
			return true;
		}

		public override void TextChanged(UISearchBar searchBar, string searchText) {
			_tableView.Source = new FilteringDataSource(searchText, (cell) => {
				InvokeOnMainThread(() => {
					_controller.NavigationController.PushViewController(new PsalmViewController {
						PsalmID = cell.ID
					}, true);
				});
			});
//			_tableView.DataSource = new FilteringDataSource(searchText);
//			_tableView.Delegate = new FilteringDelegate();
//			_tableView.Delegate = new JsonDataListDelegate<CellData>(_controller, _controller, (cell) => {
//				Console.WriteLine("Got a click on {0}", cell.Title);
//
//				InvokeOnMainThread(() => {
//					_controller.NavigationController.PushViewController(new PsalmViewController {
//					PsalmID = cell.ID
//				}, true);
//				});
//			});

			_tableView.ReloadData();
		}
	}

	public partial class PsalmsViewController : BackgroundViewController, IJsonDataSource<CellData> {
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for items that need
		// to be able to be created from a xib rather than from managed code

		public PsalmsViewController(IntPtr handle) : base (handle) {
			Initialize();
		}

		[Export ("initWithCoder:")]
		public PsalmsViewController(NSCoder coder) : base (coder) {
			Initialize();
		}

		public PsalmsViewController() : base ("PsalmsViewController", null) {
			Initialize();
		}
		
		void Initialize() {
			this.Cell = new CellData();
		}
		
		#endregion

		#region IJsonDataSource[Category] implementation
		private List<CellData> _data = new List<CellData>();

		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<CellData> JsonData {
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

		public int Level {
			get;
			set;
		}

		public CellData Cell {
			get;
			set;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			NavigationItem.Title = "Salmer";

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			CategoriesTableView.SeparatorColor = UIColor.FromRGB(217, 212, 199);

			SearchBar.Delegate = new PsalmsSearch(this, this.CategoriesTableView);
			SearchBar.BecomeFirstResponder();
		}
	}
}