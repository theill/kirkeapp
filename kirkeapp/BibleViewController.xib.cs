#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Mono.Data.Sqlite;
using System.IO;

using dk.kirkeapp.data;

#endregion

namespace dk.kirkeapp {
	public partial class BibleViewController : BackgroundViewController, IJsonDataSource<CellData> {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public BibleViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public BibleViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public BibleViewController() : base ("BibleViewController", null) {
			Initialize();
		}
		
		void Initialize() {
			this.Cell = new CellData();
		}
		
		#endregion

		#region IJsonDataSource[Library] implementation
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

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);
			
			LibrariesTableView.SeparatorColor = UIColor.FromRGB(217, 212, 199);

//			var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Stocks.db");

//			// copy to database
//			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
//			string dbfile = Path.Combine(documents, "kirkeapp.db");
//
//			if (!File.Exists(dbfile)) {
//				Console.WriteLine("1: {0}", File.Exists("kirkeapp.db"));
//				Console.WriteLine("2: {0}", File.Exists(Directory.GetParent(Environment.CurrentDirectory).FullName));
//				Console.WriteLine("3: {0}", File.Exists(Directory.GetParent(Environment.CurrentDirectory).FullName + "/kirkeapp.db"));
//				Console.WriteLine("4: {0}", File.Exists(Environment.CurrentDirectory + "/kirkeapp.db"));
//				Console.WriteLine("5: {0}", File.Exists("Databases/kirkeapp.db"));
//				Console.WriteLine("6: {0}", File.Exists(Environment.CurrentDirectory + "/Databases/kirkeapp.db"));
//
//				string srcDbFile = Environment.CurrentDirectory + "/Databases/kirkeapp.db";
//				Console.WriteLine("Copy file {0} to new directory {1}", srcDbFile, dbfile);
//				if (File.Exists(srcDbFile)) {
//					Console.WriteLine("File exists so copy it");
////					File.Copy(srcDbFile, dbfile, true);
//				} else {
//					Console.WriteLine("File does not exist");
//				}
//			} else {
//				Console.WriteLine("File is available and will not be copied to documents folder");
//			}

//			var connection = string.Format("Data Source={0}", "Databases/kirkeapp.db");
//
//			var conn = new SqliteConnection(connection);
//			using (var cmd = conn.CreateCommand()) {
//				conn.Open();
//				cmd.CommandText = "SELECT name as Name FROM libraries";
//				using (var reader = cmd.ExecuteReader()) {
//					while (reader.Read()) {
//						Console.WriteLine("Got {0}", reader["Name"]);
//					}
//				}
//			}

			this.LibrariesTableView.DataSource = new JsonDataSource<CellData>(this);
			this.LibrariesTableView.Delegate = new JsonDataListDelegate<CellData>(this, this, (cell) => {
				Log.WriteLine("Item {0} has been selected", cell.ID);

				if (this.Level < 2) {
					InvokeOnMainThread(() => {
						NavigationController.PushViewController(new BibleViewController {
							Level = this.Level + 1,
							Cell = cell
						}, true);
					});
				} else {
					using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
						Log.WriteLine("Chapter ID => {0}", cell.ID);
						var sections = db.Query<CellData>("SELECT id AS ID, content AS Title FROM sections WHERE chapter_id = ? ORDER BY ID", cell.ID);

						string html = string.Empty;
						foreach (var section in sections) {
							html += "<p>" + section.Title.Trim() + "</p>";
						}

						InvokeOnMainThread(() => {
							NavigationController.PushViewController(new WebPageViewController {
								Title = cell.Title,
								Html = html
							}, true);
						});
					}
				}
			});
		}

		public override void ViewDidAppear(bool animated) {
			base.ViewDidAppear(animated);
			Log.WriteLine("View did appear for level {0}", this.Level);

			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				_data = db.Query<CellData>(GetQueryByLevel(this.Level), this.Cell.ID);
			}

			this.LibrariesTableView.ReloadData();
		}

		private string GetQueryByLevel(int level) {
			switch (level) {
			case 1:
				return "SELECT id AS ID, name AS Title FROM books WHERE library_id = ? ORDER BY ID";
			case 2:
				return "SELECT id AS ID, IFNULL(title, 'Kapitel ' || number) AS Title, number AS Number FROM chapters WHERE book_id = ? ORDER BY ID";
			case 3:
				return "SELECT id AS ID, content AS Title FROM sections WHERE chapter_id = ? ORDER BY ID";
			default:
				return "SELECT id AS ID, name AS Title FROM libraries ORDER BY ID";
			}
		}

		private string GetTitleByLevel(int level) {
			switch (level) {
			case 0:
				return "Biblen";
			default:
				return this.Cell != null ? this.Cell.Title : "Ukendt";
			}
		}
	}
}

