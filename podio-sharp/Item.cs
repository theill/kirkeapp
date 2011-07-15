#region Using directives
using System;
using System.Collections.Generic;
using System.Json;

#endregion

namespace com.podio {
	public class Item : JsonValue, IJsonParsable<Item> {
		public int ItemID {
			get;
			set;
		}

		public string ExternalID {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public string Link {
			get;
			set;
		}

		public List<Field> Fields {
			get;
			set;
		}

		public Item() {
		}

		public override JsonType JsonType {
			get {
				return JsonType.Object;
			}
		}

		public Item Parse(JsonValue v) {
			ItemID = v.AsInt32("item_id");
			ExternalID = v.AsString("external_id");
			Title = v.AsString("title");
			Link = v.AsString("link");
			Fields = v.AsList<Field>("fields");
			return this;
		}

		public static Item FromJson(JsonValue v) {
			return new Item().Parse(v);
		}

		public override string ToString() {
			return string.Format("[Item: ItemID={0}, ExternalID={1}, Title={2}, Link={3}, Fields={4}, JsonType={5}]", ItemID, ExternalID, Title, Link, Fields, JsonType);
		}
	}
}