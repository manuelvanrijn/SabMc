namespace SabMc.Services.Notifo
{
	using System;
	using Config;
	using Model;
	using Model.Enums;

	public static class NotifoPushNotification
	{
		private static string _username;
		private static string _secret;
		private static bool _enabled;

		public static void Send(SabNzbdJob job)
		{
			string title, message, label;

			Setup();
			// Cancel
			if (_enabled == false)
				return;

			Console.WriteLine("== Sending Notifo message ==");
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

			label = "SabMc";
			if (job.MediaType == MediaType.TvShow)
				label = "SabMc.TvShow";
			if (job.MediaType == MediaType.Movie)
				label = "SabMc.Movie";

			Console.WriteLine(string.Format("title: {0}", title));
			Console.WriteLine(string.Format("message: {0}", message));

			service.Send(_username, label, title, message);
		}

		private static void Setup()
		{
			_secret = ConfigReader.Config.NotifoApiKey;
			_username = ConfigReader.Config.NotifoUsename;
			_enabled = ConfigReader.Config.NotifoEnabled;
		}
	}
}