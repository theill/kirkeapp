#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Json;

#endregion

namespace dk.kirkeapp {
	public partial class NewMessageViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public NewMessageViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public NewMessageViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public NewMessageViewController() : base ("NewMessageViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public Action MessageSent {
			get;
			set;
		}

		JsonValue SimpleValue(string externalID, string objectType, JsonValue val) {
			JsonObject a = new JsonObject();
			a.Add("external_id", new JsonPrimitive(externalID));
			a.Add("type", new JsonPrimitive(objectType));

			List<JsonValue > values = new List<JsonValue>();
			JsonObject v = new JsonObject();
			v.Add("value", val);
			values.Add(v);

			a.Add("values", new JsonArray(values.ToArray()));
			return a;
		}

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = "Ny besked";

			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Annuller", UIBarButtonItemStyle.Bordered, (sender, e) => {
				InvokeOnMainThread(() => {
					this.NavigationController.PopViewControllerAnimated(true);
				});
			});

			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem("Send", UIBarButtonItemStyle.Bordered, (sender, e) => {
				var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

				string message = MessageTextView.Text.Trim();

				List<JsonValue > fields = new List<JsonValue>();

				fields.Add(SimpleValue("title", "title", new JsonPrimitive(message)));
				fields.Add(SimpleValue("body", "text", new JsonPrimitive(message)));
				fields.Add(SimpleValue("author", "contact", new JsonPrimitive(appDelegate.ActiveContact.ProfileID)));
				fields.Add(SimpleValue("modtager", "user", new JsonPrimitive(Convert.ToInt32(appDelegate.PodioChurchProfileID))));

				JsonObject data = new JsonObject();
				data.Add("fields", new JsonArray(fields.ToArray()));

				appDelegate.PodioClient._post(string.Format("/item/app/{0}/", appDelegate.PodioMessagesAppID), data, (rsp) => {
					MessageSent.Invoke();

				}, AppDelegate.GenericErrorHandling);

			});

			// ensure blank slate
			MessageTextView.Text = "";

			MessageTextView.BecomeFirstResponder();
		}
	}
}