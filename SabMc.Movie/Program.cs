namespace SabMc.Movie
{
	using System;
	using System.IO;
	using Model;
	using Model.Enums;
	using Services.Config;
	using Services.Notifo;
	using Services.Xbmc;

	class Program
	{
		private static bool _error;

		static void Main(string[] args)
		{
			if (ConfigReader.CheckConfig() == false)
			{
				Console.WriteLine("INFO: Config file created, please fill it :)");
				Environment.Exit(1);
			}

			Console.WriteLine("== STARTING SABMC.MOVIE PROCESS ==");
			_error = false;
			
			if (args.Length >= 7)
			{
				SabNzbdJob job = new SabNzbdJob(args);
				job.Process(MediaType.Movie);

				if (job.Status == SabNzbdStatus.Ok)
				{
					Process(job);
					// send xbmc update library signal
					UpdateLibrary.UpdateVideoLibrary();
					// remove old files
					job.CleanUp();
				}
				if (_error == false)
				{
					// send notifio notification
					NotifoPushNotification.Send(job);
				}
			}
			else
			{
				Console.WriteLine("ERROR: no or to few parameters passed");
				Environment.Exit(1);
			}

			Console.WriteLine("== FINISHED SABMC.MOVIE PROCESS ==");
			if(_error)
				Environment.Exit(1);
		}

		private static void Process(SabNzbdJob job)
		{
			/*
			 * 1. find the name by the clean name of the folder
			 * 2. findout what the structure is of the folder and what to copy
			 * 3. create folder
			 * 4. move the job
			 */

			/* 1 */
			string cleanName = VideoHelper.GetCleanName(job.FolderName);
			Console.WriteLine(string.Format("INFO: Clean name: {0}", cleanName));

			/* 2 */
			ImdbHelper.GetMovie(cleanName);
			if (ImdbHelper.HasError)
			{
				Console.WriteLine(string.Format("ERROR: IMDB error: {0}", ImdbHelper.ErrorMessage));
				NotifoPushNotification.Send(MediaType.Movie, "IMDB Error", ImdbHelper.ErrorMessage);
				_error = true;
				return;
			}

			/* 3 */
			string movieRootFolder = ConfigReader.Config.MovieFolder;
			DirectoryInfo movieDirectory = new DirectoryInfo(Path.Combine(movieRootFolder, ImdbHelper.Movie.Title));
			if(movieDirectory.Exists)
			{
				// Folder already present in the movieRootFolder
				NotifoPushNotification.Send(MediaType.Movie, "Movie Error", string.Format("Movie folder: '{0}' already excists", movieDirectory.FullName));
				_error = true;
				return;
			}
			try
			{
				movieDirectory.Create();
			}
			catch(Exception e)
			{
				NotifoPushNotification.Send(MediaType.Movie, "Movie Error", string.Format("Unable to create folder: {0}", e.Message));
				_error = true;
				return;
			}
			
			/* 4 */
			if(job.MoveMovie(movieDirectory) == false)
			{
				job.Status = SabNzbdStatus.FailedMoveMovie;
			}
			job.Status = SabNzbdStatus.Ok;
		}
	}
}
