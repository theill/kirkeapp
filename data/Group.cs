#region Using directive
using System;
using System.Collections.Generic;

using com.podio;
#endregion

namespace dk.kirkeapp.data {
	public class Group {
		public int ID {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public List<Contact> Contacts {
			get;
			set;
		}
	}
}

