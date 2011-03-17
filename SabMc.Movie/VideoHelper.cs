namespace SabMc.Movie
{
	using System;
	using System.Text.RegularExpressions;

	/// <summary>
	/// Class for cleaning up the serie/movie name
	/// </summary>
	public static class VideoHelper
	{
		/// <summary>
		/// Returns a clean name of the movie
		/// </summary>
		/// <param name="name">dirty name</param>
		/// <returns>clean name</returns>
		public static string GetCleanName(string name)
		{
			name = name.Replace('.', ' ');

			name = GetMovieNameForYearMatch(name);
			name = GetMovieNameBySplittingParts(name);

			return name;
		}

		/// <summary>
		/// Strips the left part when year is found in the name
		/// </summary>
		/// <param name="movie">name with year</param>
		/// <returns>stripped name</returns>
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

		/// <summary>
		/// Remove some commonly used strings we don't need for searching the movie name
		/// </summary>
		/// <param name="movie">the dirty move name</param>
		/// <returns>clean movie name</returns>
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
			
			movie = movie.Replace(" par", "_");
			movie = movie.Replace(" par2", "_");
			movie = movie.Replace(" zip", "_");
			movie = movie.Replace(" rar", "_");
			movie = movie.Replace(" torrent", "_");

			int left = movie.Split('_')[0].Trim().Length;
			return originalName.Substring(0, left);
		}
	}
}
