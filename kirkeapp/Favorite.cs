#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public class Favorite : IJsonData {
		public string Title {
			get;
			set;
		}
		public Favorite() {
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("Title", this.Title);
			return options;
		}
		#endregion
	}
}

