#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class FavoritesViewController : BackgroundViewController, IJsonDataSource<Favorite> {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public FavoritesViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public FavoritesViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public FavoritesViewController() : base ("FavoritesViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		#region IJsonDataSource[Favorite] implementation
		private List<Favorite> _data;
		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<Favorite> JsonData {
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

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Favoritter";

			_data = new List<Favorite> { new Favorite { Title = "Fadervor" }, new Favorite { Title = "Trosbekendelsen" } };

			this.FavoritesTableView.DataSource = new JsonDataSource<Favorite>(this);
			this.FavoritesTableView.Delegate = new JsonDataListDelegate<Favorite>(this, this, (fav) => {
				Console.WriteLine("Favorite {0} has been selected", fav);
			});
		}
	}
}