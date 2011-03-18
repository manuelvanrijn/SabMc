namespace SabMc.Model
{
	/// <summary>
	/// SabMC Configuration Instance
	/// </summary>
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
		
        // MOVIE PATH
		private string movieFolder = string.Empty;

		public ConfigData()
		{
			notifoEnabled = false;
			notifoApiKey = "your_api_key";
			notifoUsename = "your_user_name";

			xbmcUpdateOnFinish = false;
			xbmcHostname = "your_xbmc_hostname";
			xbmcUsername = "your_xbmc_username";
			xbmcPassword = "your_xbmc_password";

			movieFolder = @"C:\data\movie_folder";
		}

		#region Properties Notifo
		
		/// <summary>
		/// Use Notifo Service
		/// </summary>
		public bool NotifoEnabled
		{
			get { return notifoEnabled; }
			set { notifoEnabled = value; }
		}

		/// <summary>
		/// Notifo API Key
		/// </summary>
		public string NotifoApiKey
		{
			get { return notifoApiKey; }
			set { notifoApiKey = value; }
		}

		/// <summary>
		/// Notifo Username
		/// </summary>
		public string NotifoUsename
		{
			get { return notifoUsename; }
			set { notifoUsename = value; }
		}

		#endregion

		#region Properties XBMC
		
		/// <summary>
		/// Update XBMC on Finish
		/// </summary>
		public bool XbmcUpdateOnFinish
		{
			get { return xbmcUpdateOnFinish; }
			set { xbmcUpdateOnFinish = value; }
		}

		/// <summary>
		/// XBMC Webinterface username
		/// </summary>
		public string XbmcUsername
		{
			get { return xbmcUsername; }
			set { xbmcUsername = value; }
		}

		/// <summary>
		/// XBMC Webinterface password
		/// </summary>
		public string XbmcPassword
		{
			get { return xbmcPassword; }
			set { xbmcPassword = value; }
		}

		/// <summary>
		/// XBMC Hostname/IP Address
		/// </summary>
		public string XbmcHostname
		{
			get { return xbmcHostname; }
			set { xbmcHostname = value; }
		}

		/// <summary>
		/// XBMC Port number
		/// </summary>
		public int XbmcPortnumber
		{
			get { return xbmcPortnumber; }
			set { xbmcPortnumber = value; }
		}

		#endregion

		#region Properties Movie Path
		
		/// <summary>
		/// Folder to move Movie's to when finished
		/// </summary>
		public string MovieFolder
		{
			get { return movieFolder; }
			set { movieFolder = value; }
		}

		#endregion
	}
}