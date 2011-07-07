#region Using directives
using System;
using System.Json;
using System.Collections.Generic;

#endregion

namespace com.podio {
	public static class JsonObjectExtensions {
		public static int AsInt32(this JsonValue o, string name) {
			return o.ContainsKey(name) && o[name] != null ? Convert.ToInt32(o[name].ToString()) : 0;
		}

		public static DateTime AsDateTime(this JsonValue o, string name) {
			return o.ContainsKey(name) && o[name] != null ? Convert.ToDateTime((string)o[name]) : DateTime.MinValue;
		}

		public static string AsString(this JsonValue o, string name) {
			if (o.ContainsKey(name) && o[name] != null) {
				return o[name];
			}

			return string.Empty;
		}

//		public static List<T> AsList<T>(this JsonValue o, string name) {
//			var items = new List<T>();
//			if (o.ContainsKey(name) && o[name] != null) {
//				foreach (var item in (o[name] as JsonArray)) {
//					if (item.JsonType == JsonType.String) {
//						items.Add("asdf");
//					}
//				}
//			}
//
//			return items;
//		}

		public static List<string> AsListString(this JsonValue o, string name) {
			var items = new List<string>();
			if (o.ContainsKey(name) && o[name] != null) {
				foreach (var item in (o[name] as JsonArray)) {
					items.Add((string)item);
				}
			}

			return items;
		}

	}
}

