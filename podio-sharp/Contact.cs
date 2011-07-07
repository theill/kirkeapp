using System;
using System.Json;
using System.Collections.Generic;

namespace com.podio {
	public class Contact {
		public int ProfileID {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public List<string> Mail {
			get;
			set;
		}

		public string About {
			get;
			set;
		}

		public Contact() {
		}

		public static Contact Parse(JsonValue o) {
			return new Contact {
				Name = o.AsString("name"),
				ProfileID = o.AsInt32("profile_id"),
				About = o.AsString("about"),
				Mail = o.AsListString("mail")
			};
		}
	}
}

