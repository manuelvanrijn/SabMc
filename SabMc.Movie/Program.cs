namespace SabMc.Movie
{
	using System;
	using System.Configuration;
	using System.Text.RegularExpressions;
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
			if (ConfigReader.CheckConfig() == false)
			{
				Console.WriteLine("Config file created, please fill it :)");
				Environment.Exit(1);
			}

			//if (args.Length >= 7)
			//{
			string[] testargs = new string[]
				                    	{
				                    		"C:\\MEUK\\TV Shows\\The Next Three Days (2010) 720P.X264.NL.SUBBED",
				                    		"Wanted (2008) By theknife Xvid nlsubs.nzb",
				                    		"Wanted (2008) By theknife Xvid nlsubs",
				                    		"None",
				                    		"alt.binaries.multimedia",
				                    		"0",
				                    		"0"
				                    	};

			SabNzbdJob job = new SabNzbdJob(testargs);
			job.Process(MediaType.Movie);

			if (job.Status == SabNzbdStatus.Ok)
			{
				Process(job);

				// send xbmc update library signal
				//UpdateLibrary.UpdateVideoLibrary();
			}

			// send notifio notification
			//NotifoPushNotification.Send(job);
			Console.ReadLine();
			//}
			//else
			//{
			//	Console.WriteLine("ERROR: no or to few parameters passed");
			//}
		}
		private static void Process(SabNzbdJob job)
		{
			string apiKey = ConfigReader.Config.TmdbApiKey;

			TmdbAPI theMovieDbApi = new TmdbAPI(apiKey);
			Console.WriteLine(job.FolderName);
			Console.WriteLine(CleanUp(job.FolderName));
			TmdbMovie[] results = theMovieDbApi.MovieSearch(job.FolderName);

			Console.WriteLine(string.Format("results found: {0}", results.Length));
			foreach (TmdbMovie movie in results)
			{
				Console.WriteLine(string.Format("movie: {0}", movie.Name));
			}
		}

		private static string GetMovieNameForYearMatch(string movie)
		{
			string movieName = movie;
			const string patternYear = @"(?<Year>\d{4})";

			Match titleMatchYear = Regex.Match(movie, patternYear);
			if (titleMatchYear.Success)
			{
				int year = 0;
				Int32.TryParse(titleMatchYear.Groups["Year"].Value, out year);
				string[] titleSplit = Regex.Split(movie, patternYear);
				movieName = titleSplit[0].TrimEnd(' ', '(', '.').Replace('.', ' ') + " (" + year + ")";
			}
			return movieName;
		}

		private static string CleanUp(string name)
		{
			name = GetMovieNameForYearMatch(name);
			return name;
		}
	}
}
