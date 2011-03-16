namespace SabMc.Movie
{
	using System.Threading;
	using Imdb;

	public static class ImdbHelper
	{
		private static bool _busy, _hasError;
		private static string _errorMessage, _name;
		private static Movie _result;

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
		private static void Init(string name)
		{
			_busy = true;
			_hasError = false;
			_errorMessage = string.Empty;
			_result = null;

			_name = name;
		}

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
//				DirectoryInfo di = new DirectoryInfo(Path.Combine(_movieFolder, movie.Title));
//				if (di.Exists == false)
//				{
//					di.Create();
//					job.MoveMovie(di);
//					NotifoPushNotification.Send(job);
//					UpdateLibrary.UpdateVideoLibrary();
//					Environment.Exit(0);
//				}
//				else
//				{
//					NotifoPushNotification.Send(MediaType.Movie, "Move error", string.Format("Folder: '{0}' already excists", di.FullName));
//					Environment.Exit(1);
//				}
//			}
//
//			// ehhh ?
//			NotifoPushNotification.Send(MediaType.Movie, "Error", "weird error with the imdb searcher.");
//			Environment.Exit(1);

			_busy = false;
		}

		public static bool HasError
		{
			get { return _hasError; }
		}
		public static string ErrorMessage
		{
			get { return _errorMessage; }
		}
		public static Movie Movie
		{
			get { return _result; }
		}
	}
}
