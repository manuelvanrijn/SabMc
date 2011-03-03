namespace SabMc.Model
{
	public class ConfigData
	{
		// NOTIFO
		private bool notifoEnabled;
		private string notifoApiKey = string.Empty;
		private string notifoUsename = string.Empty;

		// XBMC
		private bool xbmcUpdateOnFinish;
		private string xbmcUsername = string.Empty;
		private string xbmcPassword = string.Empty;
		private string xbmcHostname = string.Empty;
		private int xbmcPortnumber = 8080;

		// TheMovieDB
		private string tmdbApiKey = string.Empty;

		public ConfigData()
		{
			notifoEnabled = false;
			notifoApiKey = "your_api_key";
			notifoUsename = "your_user_name";

			xbmcUpdateOnFinish = false;
			xbmcHostname = "your_xbmc_hostname";
			xbmcUsername = "your_xbmc_username";
			xbmcPassword = "your_xbmc_password";

			tmdbApiKey = "your_api_key";
		}

		#region Properties Notifo
		public bool NotifoEnabled
		{
			get { return notifoEnabled; }
			set { notifoEnabled = value; }
		}

		public string NotifoApiKey
		{
			get { return notifoApiKey; }
			set { notifoApiKey = value; }
		}

		public string NotifoUsename
		{
			get { return notifoUsename; }
			set { notifoUsename = value; }
		}
		#endregion

		#region Properties XBMC
		public bool XbmcUpdateOnFinish
		{
			get { return xbmcUpdateOnFinish; }
			set { xbmcUpdateOnFinish = value; }
		}

		public string XbmcUsername
		{
			get { return xbmcUsername; }
			set { xbmcUsername = value; }
		}

		public string XbmcPassword
		{
			get { return xbmcPassword; }
			set { xbmcPassword = value; }
		}

		public string XbmcHostname
		{
			get { return xbmcHostname; }
			set { xbmcHostname = value; }
		}

		public int XbmcPortnumber
		{
			get { return xbmcPortnumber; }
			set { xbmcPortnumber = value; }
		}

		#endregion

		#region Properties The Movie DB

		public string TmdbApiKey
		{
			get { return tmdbApiKey; }
			set { tmdbApiKey = value; }
		}

		#endregion
	}
}