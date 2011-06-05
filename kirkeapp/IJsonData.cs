using System;
using System.Collections.Generic;

namespace dk.kirkeapp {
	public interface IJsonData {
		Dictionary<string, string> ToOptions();
	}
}

