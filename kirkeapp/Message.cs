#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public class Message : IJsonData {
		public string From {
			get;
			set;
		}

		public string To {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public string Content {
			get;
			set;
		}

		public DateTime SentAt {
			get;
			set;
		}

		public Message() {
		}

		public override string ToString() {
			return string.Format("[Message: From={0}, Content={1}, SentAt={2}]", From, Content, SentAt);
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("From", this.From);
			options.Add("Content", this.Content);
			options.Add("SentAt", this.SentAt.ToString());
			return options;
		}
		#endregion
	}
}