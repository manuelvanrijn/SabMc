namespace SabMc.TvShow
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using Model;
	using Model.Enums;
	using Services.Config;
	using Services.Notifo;
	using Services.Xbmc;

	class Program
	{
		static void Main(string[] args)
		{
			if(ConfigReader.CheckConfig() == false)
			{
				Console.WriteLine("INFO: Config file created, please fill it :)");
				Environment.Exit(1);
			}

			Console.WriteLine("== STARTING SABMC.TVSHOW PROCESS ==");

			if (args.Length >= 7)
			{
				SabNzbdJob job = new SabNzbdJob(args);
				job.Process(MediaType.TvShow);

				if (job.Status == SabNzbdStatus.Ok)
				{
					Process(job);
					// send xbmc update library signal
					UpdateLibrary.UpdateVideoLibrary();
					// remove old files
					job.CleanUp();
				}

				// send notifio notification
				NotifoPushNotification.Send(job);
			}
			else
			{
				Console.WriteLine("ERROR: no or to few parameters passed");
				Environment.Exit(1);
			}

			Console.WriteLine("== FINISHED SABMC.TVSHOW PROCESS ==");
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
						throw new Exception("ERROR: can't find tvrenamer.exe ?");
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
