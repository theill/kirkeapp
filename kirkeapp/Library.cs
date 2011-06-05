#region Using directives
using System;

#endregion

namespace dk.kirkeapp {
	public class Library {
		public string Name {
			get;
			set;
		}

		public Library() {
		}

		public override string ToString() {
			return string.Format("[Library: Name={0}]", Name);
		}
	}
}

