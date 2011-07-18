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

				List<JsonValue > fields = new List<JsonValue>();

				// title
				JsonObject title = new JsonObject();
				title.Add("external_id", new JsonPrimitive("title"));
				title.Add("type", new JsonPrimitive("title"));

				List<JsonValue > titleValues = new List<JsonValue>();
				JsonObject v = new JsonObject();
				v.Add("value", this.MessageTextView.Text.Trim());
				titleValues.Add(v);

				title.Add("values", new JsonArray(titleValues.ToArray()));
				fields.Add(title);

				// author
				JsonObject author = new JsonObject();
				author.Add("external_id", new JsonPrimitive("author"));
				author.Add("type", new JsonPrimitive("contact"));

				List<JsonValue > authorValues = new List<JsonValue>();
				v = new JsonObject();
				v.Add("value", new JsonPrimitive(appDelegate.ActiveContact.ProfileID));

				authorValues.Add(v);

				author.Add("values", new JsonArray(authorValues.ToArray()));
				fields.Add(author);

				// recipient
				JsonObject recipient = new JsonObject();
				recipient.Add("external_id", new JsonPrimitive("modtager"));
				recipient.Add("type", new JsonPrimitive("user"));

				List<JsonValue > recipientValues = new List<JsonValue>();
				v = new JsonObject();
				v.Add("value", new JsonPrimitive(Convert.ToInt32(appDelegate.PodioChurchProfileID)));

				recipientValues.Add(v);

				recipient.Add("values", new JsonArray(recipientValues.ToArray()));
				fields.Add(recipient);


				JsonObject data = new JsonObject();
				data.Add("fields", new JsonArray(fields.ToArray()));

				#region Example of POST data
//{
//  "external_id": The external id of the item. This can be used to hold a reference to the item in an external system.
//  "fields": The values for each field,
//
//  [
//    {
//      "field_id": The id of the field (field_id or external_id must be specified),
//      "external_id": The external id of the field (field_id or external_id must be specified),
//      "values": The values
//      [
//        {
//          "{sub_id}":{value},
//          ... (more sub_ids and values)
//        },
//        ... (more values)
//      ]
//    },
//    .... (more fields)
//  ],  "file_ids": Temporary files that have been uploaded and should be attached to this item,
//  [
//    {file_id},
//    .... (more file ids)
//  ],
//  "tags": The tags to put on the item
//  [
//    {tag}: The text of the tag to add,
//    ... (more tags)
//  ]
//}
				#endregion

				appDelegate.PodioClient._post(string.Format("/item/app/{0}/", appDelegate.PodioMessagesAppID), data, (rsp) => {

					InvokeOnMainThread(() => {
						this.NavigationController.PopViewControllerAnimated(true);
					});

				}, AppDelegate.GenericErrorHandling);

			});

			// ensure blank slate
			this.MessageTextView.Text = "";

			this.MessageTextView.BecomeFirstResponder();
		}
	}
}