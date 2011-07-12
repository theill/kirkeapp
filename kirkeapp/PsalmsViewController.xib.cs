#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public class FilteringDataSource : UITableViewDataSource {
		public static NSString kCellIdentifier = new NSString("CellIdentifier");
		private List<CellData> _data = new List<CellData>();
		private Dictionary<int, IJsonCellController> controllers = new Dictionary<int, IJsonCellController>();

		public FilteringDataSource() {
		}

		public FilteringDataSource(string filter) {
			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				if (!string.IsNullOrEmpty(filter)) {
					_data = db.Query<CellData>("SELECT id AS ID, title AS Title FROM psalms WHERE no = ? ORDER BY no", filter);
				} else {
					_data = db.Query<CellData>("SELECT id AS ID, name AS Title FROM categories ORDER BY id");
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
			_tableView.DataSource = new FilteringDataSource(searchText);
			_tableView.Delegate = new JsonDataListDelegate<CellData>(_controller, _controller, (cell) => {
				Console.WriteLine("Got a click on {0}", cell.Title);

				var c = new PsalmViewController {
					PsalmID = cell.ID
				};

				InvokeOnMainThread(() => {
					_controller.NavigationController.PushViewController(c, true);
				});
			});

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

			NavigationItem.Title = GetTitleByLevel(this.Level);

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			CategoriesTableView.SeparatorColor = UIColor.FromRGB(217, 212, 199);

			CategoriesTableView.DataSource = new JsonDataSource<CellData>(this);
			CategoriesTableView.Delegate = new JsonDataListDelegate<CellData>(this, this, (cell) => {
				if (this.Level < 1) {
					var c = new PsalmsViewController {
						Level = this.Level + 1,
						Cell = cell
					};
					NavigationController.PushViewController(c, true);
				} else {
					var c = new PsalmViewController {
						PsalmID = cell.ID
					};
					NavigationController.PushViewController(c, true);
				}
			});

			SearchBar.Delegate = new PsalmsSearch(this, this.CategoriesTableView);
		}

		public override void ViewDidAppear(bool animated) {
			base.ViewDidAppear(animated);

			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				_data = db.Query<CellData>(GetQueryByLevel(this.Level), this.Cell.ID);
			}

			CategoriesTableView.ReloadData();
		}

		private string GetQueryByLevel(int level) {
			switch (level) {
			case 1:
				return "SELECT id AS ID, (no || ' ' || title) AS Title FROM psalms WHERE category_id = ? ORDER BY no";
			case 2:
				return "SELECT id AS ID, content AS Title FROM verses WHERE psalm_id = ? ORDER BY no";
			default:
				return "SELECT id AS ID, name AS Title FROM categories ORDER BY id";
			}
		}

		private string GetTitleByLevel(int level) {
			switch (level) {
			case 0:
				return "Salmer";
			default:
				return this.Cell != null ? this.Cell.Title : "Ukendt";
			}
		}
	}
}