#region Using directives
using System;
using System.Collections.Generic;

using dk.kirkeapp.data;

#endregion

namespace dk.kirkeapp {
	public class Favorite : IJsonData {
		public string Title {
			get;
			set;
		}

		public string Content {
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

