namespace SabMc.Movie
{
	using System.Threading;
	using Imdb;

	/// <summary>
	/// Helper for finding information from IMDB
	/// </summary>
	public static class ImdbHelper
	{
		private static bool _busy, _hasError;
		private static string _errorMessage, _name;
		private static Movie _result;

		/// <summary>
		/// Returns the Movie if exactly found 1 match
		/// </summary>
		/// <param name="name">search string</param>
		/// <returns>errors?</returns>
		public static bool GetMovie(string name)
		{
			Init(name);
			Services service = new Services();
			service.FindMovie(name);
			service.FoundMovies += ServiceFoundMovies;

			while(_busy)
			{
				// wait for request to end
				Thread.Sleep(500);
			}

			return _hasError;
		}
		/// <summary>
		/// Set's default param's before starting the search
		/// </summary>
		/// <param name="name">search string</param>
		private static void Init(string name)
		{
			_busy = true;
			_hasError = false;
			_errorMessage = string.Empty;
			_result = null;

			_name = name;
		}
		/// <summary>
		/// Callback from the IMDB Service
		/// </summary>
		/// <param name="resultSet">Set with results</param>
		static void ServiceFoundMovies(MoviesResultset resultSet)
		{
			if (resultSet.Error)
			{
				_hasError = true;
				_errorMessage = string.Format("Error: {0}", resultSet.ErrorMessage);
			}
			if (resultSet.ExactMatches != null && resultSet.ExactMatches.Count == 0)
			{
				_hasError = true;
				_errorMessage = string.Format("Found no matches for: {0}", _name);
			}
			if (resultSet.ExactMatches != null && resultSet.ExactMatches.Count > 1)
			{
				_hasError = true;
				_errorMessage = string.Format("Found more than one match ({1} matches) for the querystring: {0}", _name, resultSet.ExactMatches.Count);
			}

			if (resultSet.ExactMatches != null && resultSet.ExactMatches.Count == 1)
			{
				_result = resultSet.ExactMatches[0];
				//Console.WriteLine(string.Format("Success: {0}", movie.Title));
			}
			else
			{
				// nothing found????
				_hasError = true;
				_errorMessage = string.Format("Found no matches for: {0}", _name);
			}
			_busy = false;
		}

		#region Properties

		/// <summary>
		/// Has Errors
		/// </summary>
		public static bool HasError
		{
			get { return _hasError; }
		}
		/// <summary>
		/// Error message
		/// </summary>
		public static string ErrorMessage
		{
			get { return _errorMessage; }
		}
		/// <summary>
		/// Found Movie
		/// </summary>
		public static Movie Movie
		{
			get { return _result; }
		}

		#endregion
	}
}
