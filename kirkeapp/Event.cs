#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public class Event : IJsonData {
		public int ID {
			get;
			set;
		}
		public string Title {
			get;
			set;
		}
		public DateTime ActiveAt {
			get;
			set;
		}

		public Event() {
		}

		public override string ToString() {
			return string.Format("[Event: Title={0}, ActiveAt={1}]", Title, ActiveAt);
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("Title", this.Title);
			options.Add("ActiveAt", this.ActiveAt.ToString());
			return options;
		}
		#endregion
	}
}