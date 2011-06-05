using System;

namespace dk.kirkeapp {
	public class Message {
		public string From {
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
	}
}