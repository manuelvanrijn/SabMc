namespace SabMc.Services.Xbmc
{
	using System;
	using System.Net;
	using Config;

	/// <summary>
	/// XBMC Helper class for updating the library
	/// </summary>
	public static class UpdateLibrary
	{
		private static string _username;
		private static string _password;
		private static string _hostname;
		private static int _portnumber;
		private static bool _enabled;

		/// <summary>
		/// Update the XBMC's Video Library
		/// </summary>
		/// <returns>updated</returns>
		public static bool UpdateVideoLibrary()
		{
			Setup();
			// Cancel if not enabled
			if (_enabled == false)
				return true;

			Console.WriteLine("== Sending XBMC update video library signal ==");

			try
			{
				string url = string.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", _hostname, _portnumber);
				CredentialCache credentialCache = new CredentialCache();
				credentialCache.Add(new Uri(url), "BASIC", new NetworkCredential(_username, _password));

				WebRequest request = WebRequest.Create(url);
				request.Credentials = credentialCache;
				request.GetResponse();

				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}

		/// <summary>
		/// Read's the configuration file's properties
		/// </summary>
		private static void Setup()
		{
			_username = ConfigReader.Config.XbmcUsername;
			_password = ConfigReader.Config.XbmcPassword;
			_hostname = ConfigReader.Config.XbmcHostname;
			_portnumber = ConfigReader.Config.XbmcPortnumber;
			_enabled = ConfigReader.Config.XbmcUpdateOnFinish;
		}
	}
}