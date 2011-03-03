namespace SabMc.TvShow
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using Model;
	using Model.Enums;
	using Services.Notifo;
	using Services.Xbmc;

	class Program
	{
		// "C:\MEUK\TV Shows\castle.317.720p-dimension.sample.mkv" castle.317.720p-dimension.sample.mkv.nzb castle.317.720p-dimension.sample.mkv "tv shows" alt.binaries.multimedia   0
		static void Main(string[] args)
		{
			SabNzbdJob job = new SabNzbdJob(args, MediaType.TvShow);

			if (job.Status == SabNzbdStatus.Ok)
			{
				Process(job);
				
				// send xbmc update library signal
				UpdateLibrary.UpdateVideoLibrary();
			}

			// send notifio notification
			NotifoPushNotification.Send(job);
		}
		private static void Process(SabNzbdJob job)
		{
			string appPath = AppDomain.CurrentDomain.BaseDirectory;
			string workingDirectory = Path.Combine(appPath, "Libs\\TvRenamer\\");

			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.CreateNoWindow = false;
			startInfo.UseShellExecute = false;
			startInfo.FileName = workingDirectory + "TvRenamer.exe";
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.WorkingDirectory = workingDirectory;
			
			// TVRenamer only uses args 1, 3 and 7
			startInfo.Arguments = string.Format("\"{0}\" 0 \"{1}\" 0 0 0 0", job.FullFolderName, job.FileName);
			try
			{
				using (Process exeProcess = System.Diagnostics.Process.Start(startInfo))
				{
					if (exeProcess != null)
					{
						exeProcess.WaitForExit();
						if (exeProcess.ExitCode == 0)
							job.Status = SabNzbdStatus.Ok;
						else
							job.Status = SabNzbdStatus.FailedTvRenamer;
					}
					else
					{
						throw new Exception("can't find tvrenamer.exe ?");
					}
				}
			}
			catch (Exception)
			{
				job.Status = SabNzbdStatus.FailedTvRenamer;
			}
		}
	}
}
