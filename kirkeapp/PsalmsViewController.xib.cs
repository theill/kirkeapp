#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class PsalmsViewController : BackgroundViewController, IJsonDataSource<Category> {
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
		}
		
		#endregion

		#region IJsonDataSource[Category] implementation
		private List<Category> _data;

		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<Category> JsonData {
			get {
				return _data;
			}
		}
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Salmer";

			_data = new List<Category> { new Category { Name = "Lovsange" }, new Category { Name = "Troen på Gud Fader - xxx" } };

			this.CategoriesTableView.DataSource = new JsonDataSource<Category>(this);
			this.CategoriesTableView.Delegate = new JsonDataListDelegate<Category>(this, this);
		}
	}
}

