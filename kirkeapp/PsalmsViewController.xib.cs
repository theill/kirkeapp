#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public class LeesPlankjeDataSource : UITableViewDataSource {
		public static NSString kCellIdentifier = new NSString("CellIdentifier");
		private List<CellData> _data = new List<CellData>();
		private Dictionary<int, IJsonCellController> controllers = new Dictionary<int, IJsonCellController>();

		public LeesPlankjeDataSource() {
		}

		public LeesPlankjeDataSource(string filter) {
			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				_data = db.Query<CellData>("SELECT id AS ID, title AS Title FROM psalms WHERE no = ? ORDER BY no", filter);
				Console.WriteLine("We got {0} data pieces", _data.Count);
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath) {
			Console.WriteLine("Getting cell at {0}", indexPath.Row);
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
		private UITableView _tableView;

		public PsalmsSearch(UITableView x) {
			_tableView = x;
		}

		public override void TextChanged(UISearchBar searchBar, string searchText) {
			// TODO: Implement - see: http://go-mono.com/docs/index.aspx?link=T%3aMonoTouch.Foundation.ModelAttribute
			Console.WriteLine("yes {0} and {1}", searchBar, searchText);

			_tableView.DataSource = new LeesPlankjeDataSource(searchText);
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

			this.NavigationItem.Title = GetTitleByLevel(this.Level);

			this.CategoriesTableView.DataSource = new JsonDataSource<CellData>(this);
			this.CategoriesTableView.Delegate = new JsonDataListDelegate<CellData>(this, this, (cell) => {
				Console.WriteLine("Cell {0} has been selected", cell);

				if (this.Level < 2) {
					var c = new PsalmsViewController();
					c.Level = this.Level + 1;
					c.Cell = cell;
					NavigationController.PushViewController(c, true);
				}
			});

			this.SearchBar.Delegate = new PsalmsSearch(this.CategoriesTableView);
		}

		public override void ViewDidAppear(bool animated) {
			base.ViewDidAppear(animated);
			Console.WriteLine("View did appear for level {0}", this.Level);

			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				_data = db.Query<CellData>(GetQueryByLevel(this.Level), this.Cell.ID);
			}

			this.CategoriesTableView.ReloadData();
		}

		private string GetQueryByLevel(int level) {
			switch (level) {
			case 1:
				return "SELECT id AS ID, title AS Title FROM psalms WHERE category_id = ? ORDER BY no";
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