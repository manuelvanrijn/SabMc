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
				                    		"C:\\MEUK\\TV Shows\\The Next Three Days 720P.X264.NL.SUBBED",
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
			
			string cleanName = CleanUp(job.FolderName);
			Console.WriteLine(cleanName);
			TmdbMovie[] results = theMovieDbApi.MovieSearch(cleanName);

			Console.WriteLine(string.Format("results found: {0}", results.Length));
			foreach (TmdbMovie movie in results)
			{
				if(movie.Released.HasValue)
					Console.WriteLine(string.Format("movie: {0} ({1})", movie.Name, movie.Released.Value.Year));
				else
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

		private static string GetMovieNameBySplittingParts(string movie)
		{
			string originalName = movie;

			movie = movie.ToLower().Replace(" vob", "_");
            movie = movie.Replace(" cam", "_");
            movie = movie.Replace(" dvdrip", "_");
            movie = movie.Replace(" dvdscr", "_");
            movie = movie.Replace(" dvd", "_");
            movie = movie.Replace(" r5", "_");
            movie = movie.Replace(" ts", "_");
            movie = movie.Replace(" kvcd", "_");
            movie = movie.Replace(" xvid", "_");
            movie = movie.Replace(" divx", "_");
            movie = movie.Replace(" x264", "_");
            movie = movie.Replace(" 720p", "_");
            movie = movie.Replace(" ts", "_");
            movie = movie.Replace(" ws", "_");
            movie = movie.Replace(" proper", "_");
            movie = movie.Replace(" bluray", "_");
            movie = movie.Replace(" hd-dvd", "_");
            movie = movie.Replace(" hd-screener", "_");
			movie = movie.Replace(" sub", "_");
			
			int left = movie.Split('_')[0].Trim().Length;
			return originalName.Substring(0, left);
		}

		private static string CleanUp(string name)
		{
			name = name.Replace('.', ' ');
			
			name = GetMovieNameForYearMatch(name);
			name = GetMovieNameBySplittingParts(name);

			return name;
		}
	}
}
