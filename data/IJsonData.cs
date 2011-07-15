#region Using directives
using System;
using System.Collections.Generic;

#endregion

namespace dk.kirkeapp.data {
	public interface IJsonData {
		OptionDictionary ToOptions();
	}
}