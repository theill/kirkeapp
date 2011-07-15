#region Using directives
using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

#endregion

namespace dk.kirkeapp.data {
	public interface IJsonCellController {
		UITableViewCell ViewCell { get; }

		void Configure(OptionDictionary options);
	}
}