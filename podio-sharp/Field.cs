using System;
using System.Json;
using System.Collections.Generic;

namespace com.podio {
	public class Field : JsonValue, IJsonParsable<Field> {
		public int FieldID {
			get;
			set;
		}

		public string FieldType {
			get;
			set;
		}

		public string ExternalID {
			get;
			set;
		}

		public string Label {
			get;
			set;
		}

		public List<Value> Values {
			get;
			set;
		}

		public Field() {
		}

		public override JsonType JsonType {
			get {
				return JsonType.Object;
			}
		}

		public Field Parse(JsonValue v) {
			FieldID = v.AsInt32("field_id");
			FieldType = v.AsString("type");
			ExternalID = v.AsString("external_id");
			Label = v.AsString("label");
			Values = v.AsList<Value>("values");
			return this;
		}

		public override string ToString() {
			return string.Format("[Field: FieldID={0}, FieldType={1}, ExternalID={2}, Label={3}, JsonType={4}]", FieldID, FieldType, ExternalID, Label, JsonType);
		}
	}
}