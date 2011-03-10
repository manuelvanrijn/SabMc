namespace SabMc.Movie
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using Imdb;
	using Model;
	using Model.Enums;
	using Services.Config;
	using Services.Notifo;
	using Services.Xbmc;

	class Program
	{
		private static string queryString = string.Empty;
		private static string _movieFolder = string.Empty;
		private static SabNzbdJob job;
		private static bool imdbStarted = false;

		static void Main(string[] args)
		{
			if (ConfigReader.CheckConfig() == false)
			{
				Console.WriteLine("Config file created, please fill it :)");
				Environment.Exit(1);
			}
			Setup();

			if (args.Length >= 7)
			{
				/*string[] testargs = new string[]
											{
												"C:\\MEUK\\TV Shows\\The.Next.Three.Days.720P.X264.NL.SUBBED",
												"Wanted (2008) By theknife Xvid nlsubs.nzb",
												"Wanted (2008) By theknife Xvid nlsubs",
												"None",
												"alt.binaries.multimedia",
												"0",
												"0"
											};*/

				job = new SabNzbdJob(args);
				job.Process(MediaType.Movie);

				if (job.Status == SabNzbdStatus.Ok)
					Process();
				else
					NotifoPushNotification.Send(job);
			}
			else
			{
				Console.WriteLine("ERROR: no or to few parameters passed");
			}
		}
		private static void Process()
		{
			Console.WriteLine(job.FolderName);
			queryString = CleanUp(job.FolderName);
			Console.WriteLine(queryString);

			Services imdbService = new Services();
			imdbService.FindMovie(queryString);
			imdbService.FoundMovies += new Services.FoundMoviesEventHandler(ImdbServiceFoundMovies);
			// wait for it!
			Console.ReadLine();
		}

		static void ImdbServiceFoundMovies(MoviesResultset resultset)
		{
			if (imdbStarted)
				return;

			imdbStarted = true;
			if (resultset.Error)
			{
				Console.WriteLine(string.Format("Error: {0}", resultset.ErrorMessage));
				NotifoPushNotification.Send(MediaType.Movie, "IMDB error", string.Format("Error: {0}", resultset.ErrorMessage));
				Environment.Exit(1);
			}
			if (resultset.ExactMatches != null && resultset.ExactMatches.Count == 0)
			{
				Console.WriteLine(string.Format("Found no matches for the querystring: {0}", queryString));
				NotifoPushNotification.Send(MediaType.Movie, "IMDB error", string.Format("Found no matches for the querystring: {0}", queryString));
				Environment.Exit(1);
			}
			if (resultset.ExactMatches != null && resultset.ExactMatches.Count > 1)
			{
				Console.WriteLine(string.Format("Found more than one match ({1} matches) for the querystring: {0}", queryString,
												resultset.ExactMatches.Count));
				NotifoPushNotification.Send(MediaType.Movie, "IMDB error", string.Format("Found more than one match ({1} matches) for the querystring: {0}", queryString, resultset.ExactMatches.Count));
				Environment.Exit(1);
			}

			if (resultset.ExactMatches != null && resultset.ExactMatches.Count == 1)
			{
				Movie movie = resultset.ExactMatches[0];
				Console.WriteLine(string.Format("Success: {0}", movie.Title));
				DirectoryInfo di = new DirectoryInfo(Path.Combine(_movieFolder, movie.Title));
				if (di.Exists == false)
				{
					di.Create();
					job.MoveMovie(di);
					NotifoPushNotification.Send(job);
					UpdateLibrary.UpdateVideoLibrary();
					Environment.Exit(0);
				}
				else
				{
					NotifoPushNotification.Send(MediaType.Movie, "Move error", string.Format("Folder: '{0}' already excists", di.FullName));
					Environment.Exit(1);
				}
			}

			// ehhh ?
			NotifoPushNotification.Send(MediaType.Movie, "Error", "weird error with the imdb searcher.");
			Environment.Exit(1);
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
		private static void Setup()
		{
			_movieFolder = ConfigReader.Config.MovieFolder;
		}
	}
}
