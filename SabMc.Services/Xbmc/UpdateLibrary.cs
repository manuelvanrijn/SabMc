namespace SabMc.Services.Xbmc
{
	using System;
	using System.Configuration;
	using System.Net;

	public static class UpdateLibrary
	{
		private static string _username;
		private static string _password;
		private static string _hostname;
		private static string _portnumber;

		public static bool UpdateVideoLibrary()
		{
			Setup();
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
			catch(Exception)
			{
				return false;
			}
		}
		private static void Setup()
		{
			_username = ConfigurationManager.AppSettings["xbmc_username"];
			_password = ConfigurationManager.AppSettings["xbmc_password"];
			_hostname = ConfigurationManager.AppSettings["xbmc_hostname"];
			_portnumber = ConfigurationManager.AppSettings["xbmc_potnumber"];
		}
	}
}