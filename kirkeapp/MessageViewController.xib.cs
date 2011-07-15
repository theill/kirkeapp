#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Json;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using com.podio;

using dk.kirkeapp.data;

#endregion

namespace dk.kirkeapp {
	public partial class MessageViewController : BackgroundViewController, IJsonDataSource<Message> {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public MessageViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public MessageViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public MessageViewController() : base ("MessageViewController", null) {
			Initialize();
		}
		
		void Initialize() {
			NSNotificationCenter.DefaultCenter.AddObserver (new NSString("UIKeyboardWillShowNotification"), KeyboardWillShow);
		}
		
		#endregion

		public Message PrimaryMessage { get; set; }

		public int ListCount {
			get {
				return _messages.Count;
			}
		}

		public List<Message> JsonData {
			get {
				return _messages;
			}
		}

		public string CellNibName {
			get {
				return "MessageCellViewController";
			}
		}

		List<Message> _messages = new List<Message>();

		void KeyboardWillShow(NSNotification notification) {
			var kbdBounds = (notification.UserInfo.ObjectForKey(UIKeyboard.BoundsUserInfoKey) as NSValue).RectangleFValue;

			RectangleF frame = tblMessages.Frame;
			float height = frame.Size.Height - kbdBounds.Height;

			UIView.BeginAnimations("scrollIntoView");
			UIView.SetAnimationDuration(0.3);
			viewComposing.Frame = ComputeComposerSize(kbdBounds);
			tblMessages.Frame = new RectangleF(frame.Location, new SizeF(tblMessages.Frame.Width, height));
			UIView.CommitAnimations();

			ScrollToLastRow();
		}

		RectangleF ComputeComposerSize(RectangleF kbdBounds) {
			var view = View.Bounds;

			return new RectangleF(0, view.Height - kbdBounds.Height - viewComposing.Frame.Height, viewComposing.Frame.Width, viewComposing.Frame.Height);
		}

		void ScrollToLastRow() {
			if (_messages.Count > 0) {
				tblMessages.ScrollToRow(NSIndexPath.FromRowSection(_messages.Count - 1, 0), UITableViewScrollPosition.Top, false);
			}
		}


		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = PrimaryMessage.From ?? "Ukendt";

			UIImage image = UIImage.FromBundle("Images/double-paper.png");
			UIImageView a = new UIImageView(image);
			this.View.AddSubview(a);
			this.View.InsertSubviewAbove(a, this.View.Subviews[0]);

			image = UIImage.FromBundle("Images/brown-gradient.png");
			a = new UIImageView(image);
			View.AddSubview(a);

			tblMessages.SeparatorColor = UIColor.FromRGB(217, 212, 199);

			_messages = new List<Message>();

			// insert message as first comment
			_messages.Add(PrimaryMessage);

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/comment/item/{0}/", PrimaryMessage.ID), (rsp) => {
				Console.WriteLine("Got a message: {0}", rsp);

				JsonArray items = (JsonArray)rsp;

				foreach (JsonObject item in items) {
					var m = new Message {
						Title = item.AsString("value"),
						Content = item.AsString("value"),
						From = item["created_by"].AsString("name"),
						SentAt = item.AsDateTime("created_on")
					};
					_messages.Add(m);
				}

				InvokeOnMainThread(() => {
					tblMessages.DataSource = new JsonDataSource<Message>(this);
					tblMessages.ReloadData();

					ScrollToLastRow();
				});

			}, (err) => {
				Console.WriteLine("Failed to read comments for message");

			});

			this.tblMessages.Delegate = new JsonDataListDelegate<Message>(this, this, (msg) => {});

			this.txtMessage.Started += (sender, e) => {
				Console.WriteLine("Editing has started");
			};

			this.txtMessage.ShouldReturn = (textField) => {
				// comment
				JsonObject comment = new JsonObject();
				comment.Add("external_id", new JsonPrimitive(appDelegate.ActiveContact.ProfileID.ToString()));
				comment.Add("value", new JsonPrimitive(textField.Text));

				appDelegate.PodioClient._post(string.Format("/comment/item/{0}/", PrimaryMessage.ID), comment, (rsp) => {

					InvokeOnMainThread(() => {
						_messages.Add(new Message { From = appDelegate.ActiveContact.Name, Title = textField.Text, Content = textField.Text, SentAt = DateTime.Now });
						tblMessages.ReloadData();

						ScrollToLastRow();

						textField.Text = "";
					});

				}, (err) => AppDelegate.GenericErrorHandling(err));

				// do not close keyboard box to allow user to keep entering messages
				return false;
			};
		}
	}
}

