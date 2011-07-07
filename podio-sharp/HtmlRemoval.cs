using System;

namespace com.podio {
/// <summary>
/// Methods to remove HTML from strings.
/// </summary>
	public static class HtmlRemoval {

		/// <summary>
		/// Remove HTML tags from string using char array.
		/// </summary>
		public static string StripTags(string source) {
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++) {
				char let = source[i];
				if (let == '<') {
					inside = true;
					continue;
				}
				if (let == '>') {
					inside = false;
					continue;
				}
				if (!inside) {
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}
	}
}