namespace SabMc.Services.Config
{
	using System;
	using System.IO;
	using System.Xml.Serialization;
	using Model;

	public class ConfigReader
	{
		private static ConfigData _config = null;

		public static ConfigData Config
		{
			get
			{
				if (_config == null)
					_config = GetConfigData();
				return _config;
			}
		}

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
