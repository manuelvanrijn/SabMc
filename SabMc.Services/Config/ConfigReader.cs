namespace SabMc.Services.Config
{
	using System;
	using System.IO;
	using System.Xml.Serialization;
	using Model;

	/// <summary>
	/// SabMC Configuration reader
	/// </summary>
	public class ConfigReader
	{
		private static ConfigData _config = null;

		/// <summary>
		/// The config data
		/// </summary>
		public static ConfigData Config
		{
			get
			{
				if (_config == null)
					_config = GetConfigData();
				return _config;
			}
		}

		/// <summary>
		/// Read the config data from file
		/// </summary>
		/// <returns></returns>
		private static ConfigData GetConfigData()
		{
			if (!File.Exists(ConfigFilePath))
			{
				using (FileStream fs = new FileStream(ConfigFilePath, FileMode.Create))
				{
					XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
					ConfigData sxml = new ConfigData();
					xs.Serialize(fs, sxml);
					return sxml;
				}
			}
			{
				using (FileStream fs = new FileStream(ConfigFilePath, FileMode.Open))
				{
					XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
					ConfigData sc = (ConfigData)xs.Deserialize(fs);
					return sc;
				}
			}
		}

		/// <summary>
		/// Check if config file excists 
		/// </summary>
		/// <returns>if excists</returns>
		public static bool CheckConfig()
		{
			if (!File.Exists(ConfigFilePath))
			{
				using (FileStream fs = new FileStream(ConfigFilePath, FileMode.Create))
				{
					XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
					ConfigData sxml = new ConfigData();
					xs.Serialize(fs, sxml);
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Save the config data to the configuration file
		/// </summary>
		/// <param name="config">Config data</param>
		/// <returns>saved?</returns>
		public static bool SaveConfigData(ConfigData config)
		{
			if (!File.Exists(ConfigFilePath)) return false; // don't do anything if file doesn't exist

			using (FileStream fs = new FileStream(ConfigFilePath, FileMode.Open))
			{
				XmlSerializer xs = new XmlSerializer(typeof(ConfigData));
				xs.Serialize(fs, config);
				return true;
			}
		}

		/// <summary>
		/// Get the configuration file's path
		/// </summary>
		public static string ConfigFilePath
		{
			get
			{
				string appPath = AppDomain.CurrentDomain.BaseDirectory;
				return Path.Combine(appPath, "config.xml");
			}
		}
	}
}
