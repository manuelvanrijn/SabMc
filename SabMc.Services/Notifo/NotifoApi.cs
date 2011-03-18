namespace SabMc.Services.Notifo
{
	using System;
	using System.Text;
	using System.Net;
	using System.Collections.Specialized;

	/// <summary>
	/// API Class for the Notifo Notification Service
	/// </summary>
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

		/// <summary>
		/// NotifoApi
		/// </summary>
		/// <param name="apiUsername">username</param>
		/// <param name="apiSecret">api key</param>
		public NotifoApi(string apiUsername, string apiSecret)
		{
			this.apiUsername = apiUsername;
			this.apiSecret = apiSecret;
		}

		/// <summary>
		/// Send Notification
		/// </summary>
		/// <param name="to">to username</param>
		/// <param name="label">label</param>
		/// <param name="title">title</param>
		/// <param name="message">message</param>
		/// <returns>was send</returns>
		public bool Send(string to, string label, string title, string message)
		{
			return Send(to, label, title, message, "");
		}

		/// <summary>
		/// Send Notification
		/// </summary>
		/// <param name="to">to username</param>
		/// <param name="label">label</param>
		/// <param name="title">title</param>
		/// <param name="message">message</param>
		/// <param name="link">embedded url</param>
		/// <returns>was send</returns>
		public bool Send(string to, string label, string title, string message, string link)
		{
			this.to = to;
			this.label = label;
			this.title = title;
			this.message = message;
			this.link = link;
			return Send();
		}

		/// <summary>
		/// Send the Notification
		/// </summary>
		/// <returns>was send</returns>
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