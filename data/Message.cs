#region Using directives
using System;
using System.Collections.Generic;
using System.Json;

using com.podio;
using System.Text.RegularExpressions;

#endregion

namespace dk.kirkeapp.data {
	public class Message : IJsonData {
		public int ID {
			get;
			set;
		}
		
		public string From {
			get;
			set;
		}

		public string To {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public string Content {
			get;
			set;
		}

		public DateTime SentAt {
			get;
			set;
		}

		public int AuthorProfileID {
			get;
			set;
		}

		public int RecipientProfileID {
			get;
			set;
		}

		public int RecipientGroupID {
			get;
			set;
		}

		public Message() {
		}

		public void PatchFields() {
			if (string.IsNullOrEmpty(Content)) {
				return;
			}

			if (Content.StartsWith("(via ")) {
				Match m = Regex.Match(Content, "\\(via ([^\\)]*)\\)(.*)", RegexOptions.Multiline);
				if (m.Groups.Count > 2) {
					From = m.Groups[1].Value.Trim();
					Content = m.Groups[2].Value.Trim();
				}
			}
		}

		public static string PatchContent(string fromName, string content) {
			return string.Format("(via {0}) {1}", fromName, content);
		}

		public static Message Parse(JsonObject item) {
			DateTime sentAt;
			DateTime.TryParse(item["initial_revision"].AsString("created_on"), out sentAt);

			Message m = new Message {
						ID = item.AsInt32("item_id"),
						Title = item.AsString("title"),
						SentAt = sentAt
					};

			JsonArray fields = (JsonArray)item["fields"];
			foreach (JsonObject field in fields) {
				string external_id = field.AsString("external_id");

				if (external_id == "body") {
					m.Content = HtmlRemoval.StripTags(field["values"][0].AsString("value"));
				}
				else if (external_id == "author") {
					m.From = field["values"][0]["value"].AsString("name");
					m.AuthorProfileID = field["values"][0]["value"].AsInt32("profile_id");
				}
				else if (external_id == "modtager") {
					m.To = field["values"][0]["value"].AsString("name");
					m.RecipientProfileID = field["values"][0]["value"].AsInt32("profile_id");
				}
				else if (external_id == "modtager-gruppe") {
					m.RecipientGroupID = field["values"][0]["value"].AsInt32("item_id");
				}
			}

			// set defaults
			if (!string.IsNullOrEmpty(m.Title) && !string.IsNullOrEmpty(m.Content)) {
				if (!m.Content.StartsWith(m.Title)) {
					// patch content field to show title and content if they are different
					// this is needed since we might fill out only title in some cases
					m.Content = m.Title + ": " + m.Content;
				}
			}
			else if (!string.IsNullOrEmpty(m.Title)) {
				m.Content = m.Title;
			}

			if (string.IsNullOrEmpty(m.From)) {
				m.From = "Ukendt";
			}

			if (string.IsNullOrEmpty(m.To)) {
				m.To = "Ukendt";
			}

			return m;
		}

		public override string ToString() {
			return string.Format("[Message: From={0}, Content={1}, SentAt={2}]", From, Content, SentAt);
		}

		#region IJsonData implementation
		public OptionDictionary ToOptions() {
			OptionDictionary options = new OptionDictionary();
			options.Add("ID", this.ID);
			options.Add("From", this.From);
			options.Add("To", this.To);
			options.Add("Title", this.Title);
			options.Add("Content", this.Content);
			options.Add("SentAt", this.SentAt.ToString());
			return options;
		}
		#endregion
	}
}