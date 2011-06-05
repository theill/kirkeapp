using System;

namespace dk.kirkeapp {
	public class Event {
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
	}
}