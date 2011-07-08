#region Using directives
using System;
using System.Reflection;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public class Library : IJsonData {
		public int ID {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public Library() {
		}

		public override string ToString() {
			return string.Format("[Library: Name={0}]", Name);
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
//			PropertyInfo[] properties = this.GetType().GetProperties();
//			foreach (PropertyInfo p in properties) {
//				options.Add(p.Name, p.GetValue(this, );
//			}
			options.Add("Name", this.Name);

			// done to support "Title" cell renderer
			options.Add("Title", this.Name);
			return options;
		}
		#endregion
	}
}