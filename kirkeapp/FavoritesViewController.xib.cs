#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class FavoritesViewController : BackgroundViewController, IJsonDataSource<string> {
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

		#region IJsonDataSource[System.String] implementation
		private List<string> _data;
		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<string> JsonData {
			get {
				return _data;
			}
		}
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Favoritter";

			_data = new List<string> { "Fadervor", "Trosbekendelsen" };

			this.FavoritesTableView.DataSource = new JsonDataSource<string>(this);
			this.FavoritesTableView.Delegate = new JsonDataListDelegate<string>(this, this);
		}
	}
}