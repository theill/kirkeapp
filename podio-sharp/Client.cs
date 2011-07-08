#region Using directives
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Json;

#endregion

namespace com.podio {
	public class Client {
		private string api_url;
		private string client_id;
		private string client_secret;
		private string oauth_token;

		public Client(string client_id, string client_secret) {
			this.api_url = "https://api.podio.com";
			this.client_id = client_id;
			this.client_secret = client_secret;
		}

		public void authenticate_with_credentials(string username, string password, Action<string> completed, Action<string> failed) {
//			curl -d 'grant_type=password&username=admin@kirkeapp.dk&password=belle0&client_id=kirkeapp&client_secret=8tv8mJmfXuop1zPI4z3RrUmy1RszQ4MsxzBEkYZxNdqcPDAf1Nclm2mZSeYqN3zU' -X POST https://api.podio.com/oauth/token
//			{"access_token":"7ae976e80dcfa1f07a96d3608e5a97d9850a3c7703319acaa083cf3c2482059367d020c2b5f0ee8bd17336167c498e90dab3db0f63cfafac5c7ee52149017af0","token_type":"bearer","ref":{"type":"user","id":65964},"expires_in":28799,"refresh_token":"8e1e6ed6495c8c025480afc9e65afd14d4329f24b6f6bcebd1341579a2ab171906451636d0fa83f0c4be08e61ebd0be6c1e6e976ebf542dff558743536ad0e07"}

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.api_url + "/oauth/token");
			req.Method = "POST";
			req.Accept = "application/json";
			req.ContentType = "application/x-www-form-urlencoded";

			byte[] buffer = Encoding.UTF8.GetBytes(string.Format("grant_type=password&username={0}&password={1}&client_id={2}&client_secret={3}", username, password, client_id, client_secret));
			req.ContentLength = buffer.Length;

			using (Stream s = req.GetRequestStream()) {
				s.Write(buffer, 0, buffer.Length);
				s.Close();
			}

			req.BeginGetResponse(ar => {
				try {
					var rsp = (HttpWebResponse)req.EndGetResponse(ar);
					using (Stream s = rsp.GetResponseStream()) {
//				{"access_token":"5a883a51b4e5414995c7887993023657b1c9fb67caed4f6f5ba8bbff8d5f760ca2ce59a20203743f4a0e35962ae81c5a65f412ba04ca055dd00491f3d8c38bd0","token_type":"bearer","ref":{"type":"user","id":65964},"expires_in":28799,"refresh_token":"9dc6503d6da479c62e134989498206656b6fe90f9cd7701629e8699c9fc89f763bcd2d242f26170690572088ac10581334665ed6920864fa108e0ace628c7876"}
						JsonValue v = JsonObject.Load(s);

						this.oauth_token = v["access_token"];
						completed.Invoke(this.oauth_token);
					}
				} catch (Exception x) {
					Console.WriteLine("Unable to authenticate");
					Console.WriteLine(x);

					failed.Invoke(x.Message);
				}
			}, null);
		}

		public void _get(string uri, Action<JsonValue> completed, Action<string> failed) {
//			curl -H 'Authorization: OAuth2 c37768fc5473133b2a8bd7d2c1b2ff30cd25bc2b8d2857d3b0a631671ab6bf1e119cac96aa745ebf6583272f361c9d645afac1cc620209f0631b57f6bf63296c' -X GET https://api.podio.com/space/url?url=https%3a%2f%2fkirkeapp.podio.com%2fkokkedal%2f
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.api_url + uri);
			req.Headers.Add("Authorization", string.Format("OAuth2 {0}", this.oauth_token));
			req.Method = "GET";
			req.Accept = "application/json";

			Console.WriteLine("<= {0}", uri);
			req.BeginGetResponse(ar => {
				try {
					var rsp = (HttpWebResponse)req.EndGetResponse(ar);
					using (Stream stream = rsp.GetResponseStream()) {
						JsonValue root = JsonObject.Load(stream);
						Console.WriteLine("=> {0}", root);

						completed.Invoke(root);
					}
				} catch (Exception x) {
					Console.WriteLine("Unable to GET from {0}", uri);
					Console.WriteLine(x);

					failed.Invoke(x.Message);
				}
			}, null);
		}

		public void _post(string uri, JsonValue data, Action<JsonValue> completed, Action<string> failed) {
//			curl -H 'Authorization: OAuth2 c37768fc5473133b2a8bd7d2c1b2ff30cd25bc2b8d2857d3b0a631671ab6bf1e119cac96aa745ebf6583272f361c9d645afac1cc620209f0631b57f6bf63296c' -X GET https://api.podio.com/space/url?url=https%3a%2f%2fkirkeapp.podio.com%2fkokkedal%2f
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.api_url + uri);
			req.Headers.Add("Authorization", string.Format("OAuth2 {0}", this.oauth_token));
			req.Method = "POST";
			req.ContentType = "application/json; charset=utf-8";
			req.Accept = "application/json";

			using (var sw = new StreamWriter(req.GetRequestStream())) {
				data.Save(sw);
				Console.WriteLine("Writing {0}", data.ToString());
			}

			req.BeginGetResponse(ar => {
				try {
					var rsp = (HttpWebResponse)req.EndGetResponse(ar);
					Stream stream = rsp.GetResponseStream();
					using (var streamReader = new StreamReader(stream)) {
						JsonValue root = JsonObject.Load(streamReader);

						completed.Invoke(root);
					}
				} catch (Exception x) {
					Console.WriteLine("Unable to POST to {0}", uri);
					Console.WriteLine(x);

					failed.Invoke(x.Message);
				}
			}, null);
		}

	}
}