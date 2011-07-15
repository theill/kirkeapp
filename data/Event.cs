#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp.data {
	public class Event : IJsonData {
		public int ID {
			get;
			set;
		}
		public string Title {
			get;
			set;
		}

		public DateTime ActiveStartAt {
			get;
			set;
		}

		public DateTime ActiveEndAt {
			get;
			set;
		}

		public Event() {
		}

		public override string ToString() {
			return string.Format("[Event: Title={0}, ActiveStartAt={1}, ActiveEndAt={2}]", Title, ActiveStartAt, ActiveEndAt);
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("Title", this.Title);
			options.Add("ActiveStartAt", this.ActiveStartAt.ToString());
			options.Add("ActiveEndAt", this.ActiveEndAt.ToString());
			return options;
		}
		#endregion
	}
}