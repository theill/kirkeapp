#region Using directives
using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp {
	public interface IJsonCellController {
		UITableViewCell ViewCell { get; }

		void Configure(OptionDictionary options);
	}
}