#region Using directives
using System;
using System.Json;

#endregion

namespace com.podio {
	public class Value : JsonValue, IJsonParsable<Value> {
		public Value() {
		}

		public JsonValue ObjectValue {
			get;
			set;
		}

		public override JsonType JsonType {
			get {
				return JsonType.Object;
			}
		}

		public Value Parse(JsonValue v) {
			ObjectValue = v;
			return this;
		}

		public override string ToString() {
			return string.Format("[Value: ObjectValue={0}, JsonType={1}]", ObjectValue, JsonType);
		}
	}
}

