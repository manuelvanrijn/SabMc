namespace SabMc.Services.Notifo
{
	using System.Configuration;
	using Model;
	using Model.Enums;

	public static class NotifoPushNotification
	{
		private static string _username;
		private static string _secret;

		public static void Send(SabNzbdJob job)
		{
			string title;
			string message;

			Setup();
			NotifoApi service = new NotifoApi(_username, _secret);
			
			switch (job.Status)
			{
				case SabNzbdStatus.FailedUnpacking:
					title = "Unpack error";
					message = string.Format("Unable to unpack {0}", job.FolderName);
					break;
				case SabNzbdStatus.FailedTvRenamer:
					title = "Rename error";
					message = string.Format("Failed to rename and move the TV show {0}", job.FolderName);
					break;
				case SabNzbdStatus.FailedVerification:
					title = "Verification error";
					message = string.Format("Verification error occured on {0}", job.FolderName);
					break;
				default:
					title = "Success";
					message = string.Format("{0} was successfully renamed and moved to xbmc", job.FileName);
					break;
			}
			
			service.Send(_username, "Sabnzbd", title, message);
		}

		private static void Setup()
		{
			_secret = ConfigurationManager.AppSettings["notifo_api_key"];
			_username = ConfigurationManager.AppSettings["notifo_usename"];
		}
	}
}