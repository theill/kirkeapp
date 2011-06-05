using System;
using System.Collections.Generic;

namespace dk.kirkeapp {
	public class Category : IJsonData {
		public string Name {
			get;
			set;
		}
		public Category() {
		}

		public override string ToString() {
			return string.Format("[Category: Name={0}]", Name);
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("Name", this.Name);

			// done to support "Title" cell renderer
			options.Add("Title", this.Name);
			
			return options;
		}
		#endregion
	}
}

