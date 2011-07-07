#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

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

			UIView.BeginAnimations("scrollIntoView");
			UIView.SetAnimationDuration(0.3);
			viewComposing.Frame = ComputeComposerSize(kbdBounds);
			UIView.CommitAnimations();
		}

		RectangleF ComputeComposerSize(RectangleF kbdBounds) {
			var view = View.Bounds;

			return new RectangleF(0, view.Height - kbdBounds.Height - viewComposing.Frame.Height, viewComposing.Frame.Width, viewComposing.Frame.Height);
		}


		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.Title = PrimaryMessage.From;

			this.tblMessages.DataSource = new JsonDataSource<Message>(this);

			this.txtMessage.Started += (sender, e) => {
				Console.WriteLine("Editing has started");
			};

			this.txtMessage.ShouldReturn = (textField) => {
				textField.ResignFirstResponder();
				Console.WriteLine("TODO: User entered {0}", textField.Text);
				return true;
			};
		}
	}
}

