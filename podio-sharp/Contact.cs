#region Using directives
using System;
using System.Json;
using System.Collections.Generic;

#endregion

namespace com.podio {
	public class Contact {
		public string Name {
			get;
			set;
		}

		public string ExternalID {
			get;
			set;
		}

		public int SpaceID {
			get;
			set;
		}

		public int ProfileID {
			get;
			set;
		}

		public List<string> Phones {
			get;
			set;
		}

		public List<string> Mails {
			get;
			set;
		}

		public Contact() {
		}

		public static Contact FromJson(JsonValue o) {
			// {"name": "John Doe", "external_id": null, "space_id": 55853, "profile_id": 2232002, "phone": ["+45 61715096"], "link": null, "mail": ["john@doe.com"], "type": "space", "image": null}
			return new Contact {
				Name = o.AsString("name"),
				ExternalID = o.AsString("external_id"),
				SpaceID = o.AsInt32("space_id"),
				ProfileID = o.AsInt32("profile_id"),
				Phones = o.AsListString("phone"),
				Mails = o.AsListString("mail")
			};
		}
	}
}