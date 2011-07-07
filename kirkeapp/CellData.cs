using System;

namespace dk.kirkeapp {
	public class CellData : IJsonData {
		public int ID {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("ID", this.ID);

			// done to support "Title" cell renderer
			options.Add("Title", this.Title);
			return options;
		}

		#endregion

		public CellData() {
		}
	}
}

