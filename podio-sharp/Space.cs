#region Using directives
using System;
using System.Json;
#endregion

namespace com.podio {
	public class Space {
		public int SpaceID {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public string Url {
			get;
			set;
		}

		public string UrlLabel {
			get;
			set;
		}

		public DateTime CreatedOn {
			get;
			set;
		}


		public Space() {
		}

		public static Space FromJson(JsonValue v) {
			// {"post_on_new_member": true, "url_label": "kokkedal", "created_on": "2011-06-13 05:51:54", "org": {"type": "free", "premium": false, "name": "Kirkeapp", "logo": null, "url": "https://kirkeapp.podio.com/", "url_label": "kirkeapp", "image": null, "org_id": 20617}, "subscribed": true, "name": "Kokkedal", "post_on_new_app": true, "rights": ["add_app", "add_contact", "add_status", "add_task", "add_widget", "subscribe", "view"], "url": "https://kirkeapp.podio.com/kokkedal/", "space_id": 55853, "org_id": 20617, "created_by": {"link": "https://podio.com/-/contacts/5183", "avatar": 2596, "user_id": 5183, "image": {"link": "https://download.podio.com/2596", "file_id": 2596}, "profile_id": 5183, "type": "user", "name": "Peter Theill"}, "role": "regular"}
			return new Space {
				SpaceID = v.AsInt32("space_id"),
				Name = v.AsString("name"),
				Url = v.AsString("url"),
				UrlLabel = v.AsString("url_label"),
				CreatedOn = v.AsDateTime("created_on")
			};
		}
	}
}