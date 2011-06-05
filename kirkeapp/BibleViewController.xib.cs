using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace dk.kirkeapp {
	public partial class BibleViewController : BackgroundViewController, IJsonDataSource<Library> {
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
		}
		
		#endregion

		#region IJsonDataSource[Library] implementation
		private List<Library> _data;
		public int ListCount {
			get {
				return _data.Count;
			}
		}

		public List<Library> JsonData {
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

			this.NavigationItem.Title = "Biblen";

			_data = new List<Library>() { new Library { Name = "Det Gamle Testamente" }, new Library { Name = "De Apokryfe Skrifter (GT)" },
				new Library { Name = "Det Nye Testamente" } };

			this.LibrariesTableView.DataSource = new JsonDataSource<Library>(this);
			this.LibrariesTableView.Delegate = new JsonDataListDelegate<Library>(this, this);
		}
	}
}

