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

			NavigationItem.Title = "Log ind";

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			DescriptionLabel.Text = string.Format("Hvis du er tilknyttet kirken har du mulighed for skrive og modtage beskeder direkte fra præsten.\n\nKontakt {0} hvis du ønsker at blive oprettet som bruger.", appDelegate.ApplicationName);

			NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Annuller", UIBarButtonItemStyle.Plain, (sender, e) => {
				InvokeOnMainThread(() => {
					this.NavigationController.PopViewControllerAnimated(true);
				});
			});

			EmailTextField.BecomeFirstResponder();

			EmailTextField.ShouldReturn = (textField) => {
				PasswordTextField.BecomeFirstResponder();
				return true;
			};

			PasswordTextField.ShouldReturn = (textField) => {
				LogIn();
				return textField.ResignFirstResponder();
			};
		}

		void LogIn() {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
			appDelegate.PodioClient._get(string.Format("/contact/space/{0}/?contact_type=space&type=full", appDelegate.ActiveSpace.SpaceID), (rsp) => {
				List<Contact > contacts = new List<Contact>();
				foreach (var v in (rsp as JsonArray)) {
					contacts.Add(Contact.FromJson(v));
				}

				Contact contact = contacts.Find((c) => {
					// FIXME: doesn't take upper/lower into consideration
					return c.Mails.Contains(this.EmailTextField.Text);
				});

				if (contact != null) {
					appDelegate.ActiveContact = contact;

					// store contact in app settings so user doesn't have to log on next time
					AppDelegate.Defaults.SetInt(contact.ProfileID, "profile_id");

					InvokeOnMainThread(() => {
						this.NavigationController.PopViewControllerAnimated(true);
					});
				} else {
					Log.WriteLine("Did not find contact");

					InvokeOnMainThread(() => {
						UserNotFoundLabel.Hidden = false;
					});
				}

			}, AppDelegate.GenericErrorHandling);
		}
	}
}