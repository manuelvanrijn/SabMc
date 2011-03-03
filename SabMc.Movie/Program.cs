namespace SabMc.Movie
{
	using System;
	using System.Configuration;
	using Model;
	using Model.Enums;
	using Services.Config;
	using Services.Notifo;
	using Services.Xbmc;
	using TheMovieDB;

	class Program
	{
		static void Main(string[] args)
		{
			if(ConfigReader.CheckConfig() == false)
			{
				Console.WriteLine("Config file created, please fill it :)");
				Environment.Exit(1);
			}

			if (args.Length >= 7)
			{
				string[] testargs = new string[]
				                    	{
				                    		"C:\\MEUK\\TV Shows\\Wanted (2008) By theknife Xvid nlsubs",
				                    		"Wanted (2008) By theknife Xvid nlsubs.nzb",
				                    		"Wanted (2008) By theknife Xvid nlsubs",
				                    		"None",
				                    		"alt.binaries.multimedia",
				                    		"0",
				                    		"0"
				                    	};

				SabNzbdJob job = new SabNzbdJob(testargs, MediaType.Movie);

				if (job.Status == SabNzbdStatus.Ok)
				{
					Process(job);

					// send xbmc update library signal
					//UpdateLibrary.UpdateVideoLibrary();
				}

				// send notifio notification
				//NotifoPushNotification.Send(job);
				Console.ReadLine();
			}
			else
			{
				Console.WriteLine("no parameters passed");
			}
		}
		private static void Process(SabNzbdJob job)
		{
			string apiKey = ConfigReader.Config.TmdbApiKey;

			TmdbAPI theMovieDbApi = new TmdbAPI(apiKey);
			TmdbMovie[] results = theMovieDbApi.MovieSearch(job.FolderName);
			System.Console.WriteLine(string.Format("results found: {0}", results.Length));
			foreach (TmdbMovie movie in results)
			{
				System.Console.WriteLine(string.Format("movie: {0}", movie.Name));
			}
		}
	}
}
