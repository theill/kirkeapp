using System;

namespace dk.kirkeapp {
	public class Category {
		public string Name {
			get;
			set;
		}
		public Category() {
		}

		public override string ToString() {
			return string.Format("[Category: Name={0}]", Name);
		}
	}
}

