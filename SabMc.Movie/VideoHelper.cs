namespace SabMc.Movie
{
	using System;
	using System.Collections.Generic;
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
			name = StripUnusedChars(name);
			name = GetMovieNameForYearMatch(name);
			name = GetMovieNameBySplittingParts(name);

			return name;
		}

		/// <summary>
		/// Returns a string without weird chars
		/// </summary>
		/// <param name="movie">dirty name</param>
		/// <returns>clean name</returns>
		private static string StripUnusedChars(string movie)
		{
			movie = movie.Replace(".", " ");
			movie = movie.Replace("?", "");
			movie = movie.Replace(">", "");
			movie = movie.Replace("<", "");
			movie = movie.Replace("*", "");
			movie = movie.Replace("|", "");
			movie = movie.Replace("\"", "");
			movie = movie.Replace(":", "");
			movie = movie.Replace(@"\", "");
			movie = movie.Replace("/", "");
			movie = movie.Replace(",", "");
			movie = movie.Replace("'", "");
			movie = movie.Replace("[", "");
			movie = movie.Replace("]", "");
			movie = movie.Replace("---", "-");
			movie = movie.Replace("  ", " ");

			return movie;
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
			movie = movie.ToLower();
			
			movie = RemoveListOccurences(movie, VideoCodecs);
			movie = RemoveListOccurences(movie, AudioCodecs);
			movie = RemoveListOccurences(movie, Resolutions);
			movie = RemoveListOccurences(movie, Sources);
			movie = RemoveListOccurences(movie, OtherStrings);
			
			// find the first occurence and take the left part of it!
			int left = movie.Split('_')[0].Trim().Length;
			return originalName.Substring(0, left).Trim();
		}

		/// <summary>
		/// Removes matches from a list from the string
		/// </summary>
		/// <param name="movie">search string</param>
		/// <param name="list">list with matches to search for</param>
		/// <returns>cleanedup string</returns>
		private static string RemoveListOccurences(string movie, IEnumerable<string> list)
		{
			foreach (string s in list)
			{
				// TODO: with space ?
				movie = movie.ToLower().Replace(string.Format(" {0}", s), "_");
			}
			return movie;
		}

		#region Derrived Properties
		/// <summary>
		/// List of video codecs
		/// </summary>
		internal static List<string> VideoCodecs
		{
			get { return new List<string>(new string[] { "divx6", "divx", "xvid", "h264", "x264", "h.264", "kvcd", "stv", "dxva" }); }
		}
		/// <summary>
		/// List of audio codecs
		/// </summary>
		internal static List<string> AudioCodecs
		{
			get { return new List<string>(new string[] { "ac3", "dts", "dd", "aac", "engdts", "freac3" }); }
		}
		/// <summary>
		/// List of different resolutions
		/// </summary>
		internal static List<string> Resolutions
		{
			get { return new List<string>(new string[] { "720p", "1080p", "1080i", "hd", "576", "720", "1080" }); }
		}
		/// <summary>
		/// List of sources
		/// </summary>
		internal static List<string> Sources
		{
			get { return new List<string>(new string[] { "tvrip", "pal", "ntsc", "bdrip", "blurayrip", "blu-ray", "bd-rip", "dvdr", "dvd5", "dvd9", "hdtv", "cam", "dvdscr", "dvd", "r5", "ts", "ws", "proper", "hd-dvd", "hddvd", "hd-screener", "hdscreener", "vob", "dl", "dd", "dd5 1", "5 1", "L4 1", "rip", "scr", "screener", "wmv", "oar", "mpeg", "dsr", "r1", "r2", "r3", "r4", "r5", "bd5", "bd9", "dtv", "stv", "xvid", "divx", "x264", "2in1", "limited", "proper", "fixed", "repack", "rerip", "retail", "extended", "remastered", "unrated", "chrono", "uncut" }); }
		}
		/// <summary>
		/// List with strings to replace
		/// </summary>
		internal static List<string> OtherStrings
		{
			get { return new List<string>(new string[] { "sub", "par", "par2", "zip", "rar", "torrent" }); }
		}

		#endregion
	}
}
