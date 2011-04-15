namespace SabMc.Services.TheRenamer
{
	using System;
	using System.Diagnostics;
	using Config;
	using Helpers;
	using Model;
	using Model.Enums;

	/// <summary>
	/// The Renamer Helper Class
	/// </summary>
	public class TheRenamer
	{
		/// <summary>
		/// Process the SabNzbdJob with TheRenamer
		/// </summary>
		/// <param name="job">The SabNzbd Job</param>
		/// <returns>The SabNzbd</returns>
		public static SabNzbdJob Process(SabNzbdJob job)
		{
			switch (job.MediaType)
			{
				case MediaType.TvShow:
					job = TvShow(job);
					break;
				case MediaType.Movie:
					job = Movie(job);
					break;
				default:
					break;
			}
			return job;
		}
		/// <summary>
		/// Processes the job as TvShow
		/// </summary>
		/// <param name="job">The SabNzbd</param>
		/// <returns>The SabNzbd</returns>
		private static SabNzbdJob TvShow(SabNzbdJob job)
		{
			string args = string.Format("-fetch -ff=\"{0}\" -af=\"{1}\"", job.FullFolderName, ConfigReader.Config.TvShowFolder);
			return StartProcessing(job, args);
		}
		/// <summary>
		/// Processes the job as a Movie
		/// </summary>
		/// <param name="job">The SabNzbd</param>
		/// <returns>The SabNzbd</returns>
		private static SabNzbdJob Movie(SabNzbdJob job)
		{
			string args = string.Format("-fetchmovie -ff=\"{0}\" -af=\"{1}\"", job.FullFolderName, ConfigReader.Config.MovieFolder);
			return StartProcessing(job, args);
		}
		/// <summary>
		/// The main process
		/// </summary>
		/// <param name="job">The SabNzbd</param>
		/// <param name="args">Arguments for theRenamer.exe</param>
		/// <returns></returns>
		private static SabNzbdJob StartProcessing(SabNzbdJob job, string args)
		{
			string workingDirectory = ConfigReader.Config.TheRenamerFolder;
			DebugHelper.WriteHeader("Start theRename process");
			DebugHelper.Info(string.Format("theRename executable path: {0}", workingDirectory));

			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.CreateNoWindow = false;
			startInfo.UseShellExecute = false;
			startInfo.FileName = workingDirectory + "\\theRenamer.exe";
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.WorkingDirectory = workingDirectory;
			startInfo.Arguments = args;
			try
			{
				using (Process exeProcess = System.Diagnostics.Process.Start(startInfo))
				{
					if (exeProcess != null)
					{
						exeProcess.WaitForExit();
						job.Status = exeProcess.ExitCode == 0 ? SabNzbdStatus.Ok : SabNzbdStatus.FailedTvRenamer;
					}
					else
					{
						throw new Exception("can't find theRenamer.exe?");
					}
				}
			}
			catch (Exception e)
			{
				DebugHelper.Error(e.ToString());
				job.Status = SabNzbdStatus.FailedTvRenamer;
			}
			
			return job;
		}
	}
}
