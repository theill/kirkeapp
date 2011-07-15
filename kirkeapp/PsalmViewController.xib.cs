#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public partial class PsalmViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public PsalmViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public PsalmViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public PsalmViewController() : base ("PsalmViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public int PsalmID {
			get;
			set;
		}

		public Psalm Psalm {
			get;
			set;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			string text = string.Empty;

			using (var db = new SQLite.SQLiteConnection("Databases/kirkeapp.db")) {
				var psalms = db.Query<Psalm>("SELECT id AS ID, no AS No, title AS Title, melody AS Melody, author AS Author FROM psalms WHERE id = ?", this.PsalmID);
				if (psalms != null && psalms.Count > 0) {
					this.Psalm = psalms[0];

					var verses = db.Query<Verse>("SELECT no AS No, content AS Content FROM verses WHERE psalm_id = ? ORDER BY no", this.PsalmID);
					text = string.Join("", verses.ConvertAll((verse) => {
						return string.Format("{0}\n{1}\n\n", verse.No, verse.Content);
					}));
				}
			}

			NavigationItem.Title = Psalm.No.ToString();

			UIImage image = UIImage.FromBundle("Images/brown-gradient.png");
			UIImageView a = new UIImageView(image);
			View.AddSubview(a);

			NoLabel.Text = Psalm.No.ToString();
			CaptionLabel.Text = Psalm.Title;
			AuthorLabel.Text = Psalm.Author;
			VersesTextView.Text = text;

			VersesTextView.Frame = new System.Drawing.RectangleF(10, 100, 274, VersesTextView.ContentSize.Height);
			ContentScrollView.ContentSize = new System.Drawing.SizeF(304, 120 + VersesTextView.ContentSize.Height);
		}
	}
}