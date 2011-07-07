#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Json;
using com.podio;

#endregion

namespace dk.kirkeapp {
	public partial class LogOnViewController : BackgroundViewController {
		#region Constructors
		
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		
		public LogOnViewController(IntPtr handle) : base (handle) {
			Initialize();
		}
		
		[Export ("initWithCoder:")]
		public LogOnViewController(NSCoder coder) : base (coder) {
			Initialize();
		}
		
		public LogOnViewController() : base ("LogOnViewController", null) {
			Initialize();
		}
		
		void Initialize() {
		}
		
		#endregion

		public override void ViewDidLoad() {
			base.ViewDidLoad();

			this.NavigationItem.HidesBackButton = true;

			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Annuller", UIBarButtonItemStyle.Plain, (sender, e) => {
				InvokeOnMainThread(() => {
					this.NavigationController.PopViewControllerAnimated(true);
				});
			});

			this.EmailTextField.BecomeFirstResponder();

			this.EmailTextField.ShouldReturn = (textField) => {
				Console.WriteLine("Bip bip - next field");
				return true;
			};

			this.PasswordTextField.ShouldReturn = (textField) => {
				LogIn();
				return textField.ResignFirstResponder();
			};
		}

		void LogIn() {
			Console.WriteLine("Logging in");

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/contact/space/{0}/?contact_type=space&type=full", appDelegate.ActiveSpace.SpaceID), (rsp) => {
				Console.WriteLine("Got list of space contacts");

				List<Contact > contacts = new List<Contact>();
				foreach (var v in (rsp as JsonArray)) {
					contacts.Add(Contact.Parse(v));
				}

				Contact contact = contacts.Find((c) => {
					return c.Mail.Contains(this.EmailTextField.Text);
				});

				if (contact != null) {
					Console.WriteLine("Found contact!");

					appDelegate.ActiveContact = contact;

					InvokeOnMainThread(() => {
						this.NavigationController.PopViewControllerAnimated(true);
					});
				} else {
					Console.WriteLine("Did not find contact");
				}

			}, (err) => {
				Console.WriteLine("Desvaerre, det gik ikke .. fik {0}", err);

				// FIXME: show error for user
			});

		}
	}
}