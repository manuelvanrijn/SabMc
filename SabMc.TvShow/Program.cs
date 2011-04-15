namespace SabMc.TvShow
{
	using System;
	using Model;
	using Model.Enums;
	using Services.Config;
	using Services.Helpers;
	using Services.Notifo;
	using Services.TheRenamer;
	using Services.Xbmc;

	class Program
	{
		static void Main(string[] args)
		{
			if(ConfigReader.CheckConfig() == false)
			{
				DebugHelper.WriteHeader("Config file generated");
				DebugHelper.Info("Config file created, please fill it :)");
				Environment.Exit(1);
			}

			DebugHelper.WriteArray("Passed arguments", args);
			
			DebugHelper.WriteHeader("Starting SabMC.TVShow Process");

			if (args.Length >= 7)
			{
				DebugHelper.Log("Found enough arguments");
				
				SabNzbdJob job = new SabNzbdJob(args);
				DebugHelper.Info(string.Format("Initial SabNzbd status code: {0}", job.Status));
				DebugHelper.Log("Start processing the job");
				job.Process(MediaType.TvShow);
				
				string cleanMovieName = VideoHelper.GetCleanName(job.FolderName);
				if (job.Status == SabNzbdStatus.Ok)
				{
					DebugHelper.Log("Job status = OK before processing");
					job = TheRenamer.Process(job);
					if (job.Status == SabNzbdStatus.Ok)
					{
						DebugHelper.Log("Job status = OK after processing");
						UpdateLibrary.UpdateVideoLibrary();
						job.CleanUp();
					}
				}
				NotifoPushNotification.Send(job, cleanMovieName);
			}
			else
			{
				DebugHelper.Error("No, or to few arguments where passed");
				Environment.Exit(1);
			}

			DebugHelper.WriteHeader("Finished SabMC.TvShow process");
		}
	}
}
