#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp {
	public interface IJsonData {
		OptionDictionary ToOptions();
	}
}