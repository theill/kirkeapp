using System;
using System.Collections.Generic;

namespace dk.kirkeapp.data {
	public interface IJsonDataSource <T> {
		int ListCount { get; }

		List<T> JsonData { get; }

		string CellNibName { get; }
	}
}

