namespace SabMc.Services.Notifo
{
	using Config;
	using Helpers;
	using Model;
	using Model.Enums;

	/// <summary>
	/// SabMC Notifo Helper
	/// </summary>
	public static class NotifoPushNotification
	{
		private static string _username;
		private static string _secret;
		private static bool _enabled;
		
		/// <summary>
		/// Send Message for SabNzbdJob
		/// </summary>
		/// <param name="job">SabNZBD Job</param>
		public static void Send(SabNzbdJob job)
		{
			switch (job.Status)
			{
				case SabNzbdStatus.FailedUnpacking:
				case SabNzbdStatus.FailedTvRenamer:
				case SabNzbdStatus.FailedVerification:
				case SabNzbdStatus.FailedMoveMovie:
					Send(job, job.FolderName);
					break;
				default:
					if (job.MediaType == MediaType.Other)
						Send(job, job.FolderName);
					else
						Send(job, job.FolderName);
					break;
			}
		}

		/// <summary>
		/// Send Message for SabNzbdJob with custom job name
		/// </summary>
		/// <param name="job">SabNZBD Job</param>
		/// <param name="jobName">Custom job name</param>
		public static void Send(SabNzbdJob job, string jobName)
		{
			string title, message;

			switch (job.Status)
			{
				case SabNzbdStatus.FailedUnpacking:
					title = "Unpack error";
					message = string.Format("Unable to unpack {0}", jobName);
					break;
				case SabNzbdStatus.FailedTvRenamer:
					title = "Rename error";
					message = string.Format("Failed to rename and move the TV show {0}", jobName);
					break;
				case SabNzbdStatus.FailedVerification:
					title = "Verification error";
					message = string.Format("Verification error occured on {0}", jobName);
					break;
				case SabNzbdStatus.FailedMoveMovie:
					title = "Move error";
					message = string.Format("Failed to move folder: {0}", jobName);
					break;
				default:
					title = "Success";
					if(job.MediaType == MediaType.Other)
						message = string.Format("{0} was successfully downloaded", jobName);
					else
						message = string.Format("{0} was successfully renamed and moved to xbmc", jobName);
					break;
			}
			Send(job.MediaType, title, message);
		}
		
		/// <summary>
		/// Send Message for Parameters
		/// </summary>
		/// <param name="mediaType">The mediatype</param>
		/// <param name="title">The title</param>
		/// <param name="message">The Message</param>
		public static void Send(MediaType mediaType, string title, string message)
		{
			Setup();
			if (_enabled == false)
				return;

			DebugHelper.WriteHeader("Sending Notifo message");
			NotifoApi service = new NotifoApi(_username, _secret);
			
			string label = "SabMc";
			if (mediaType == MediaType.TvShow)
				label = "SabMc.TvShow";
			if (mediaType == MediaType.Movie)
				label = "SabMc.Movie";

			DebugHelper.Log(string.Format("title: {0}", title), true);
			DebugHelper.Log(string.Format("message: {0}", message), true);

			service.Send(_username, label, title, message);
		}

		/// <summary>
		/// Read configuration properties
		/// </summary>
		private static void Setup()
		{
			_secret = ConfigReader.Config.NotifoApiKey;
			_username = ConfigReader.Config.NotifoUsename;
			_enabled = ConfigReader.Config.NotifoEnabled;
		}
	}
}