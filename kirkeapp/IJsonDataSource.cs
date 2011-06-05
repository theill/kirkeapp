using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace dk.kirkeapp {
	public interface IJsonDataSource <T> {
		int ListCount { get; }

		List<T> JsonData { get; }
	}
}

