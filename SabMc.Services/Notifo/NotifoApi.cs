using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;

namespace SabMc.Services.Notifo
{
	public class NotifoApi
	{
		private const string ApiUrl = "https://api.notifo.com/v1/send_notification";
		private readonly string apiUsername;
		private readonly string apiSecret;

		private string to = string.Empty;
		private string message = string.Empty;
		private string title = string.Empty;
		private string label = string.Empty;
		private string link = string.Empty;

		public NotifoApi(string apiUsername, string apiSecret)
		{
			this.apiUsername = apiUsername;
			this.apiSecret = apiSecret;
		}

		public bool Send(string to, string label, string title, string message)
		{
			return Send(to, label, title, message, "");
		}
		public bool Send(string to, string label, string title, string message, string link)
		{
			this.to = to;
			this.label = label;
			this.title = title;
			this.message = message;
			this.link = link;
			return this.Send();
		}

		protected bool Send()
		{
			WebClient wb = new WebClient();
			String auth = apiUsername + ":" + apiSecret;

			UTF8Encoding encoding = new UTF8Encoding();
			byte[] bytes = encoding.GetBytes(auth);
			auth = Convert.ToBase64String(bytes);

			wb.Headers.Add("Authorization", "Basic " + auth);

			NameValueCollection nvc = new NameValueCollection();
			nvc.Add("to", to);
			nvc.Add("msg", message);
			nvc.Add("label", label);
			nvc.Add("title", title);
			nvc.Add("uri", link);
			
			//send message
			try
			{
				byte[] apiResponse = wb.UploadValues(ApiUrl, "POST", nvc);
				Encoding.ASCII.GetString(apiResponse, 0, apiResponse.Length);
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to send message (" + ex.Message + ")");
			}

			return true;
		}

	}
}