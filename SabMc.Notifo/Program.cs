namespace SabMc.Notifo
{
	using System;
	using Model;
	using Services.Config;
	using Services.Helpers;
	using Services.Notifo;

	class Program
	{
		static void Main(string[] args)
		{
			if (ConfigReader.CheckConfig() == false)
			{
				DebugHelper.WriteHeader("Config file generated");
				DebugHelper.Info("Config file created, please fill it :)");
				Environment.Exit(1);
			}

			DebugHelper.WriteArray("Passed arguments", args);
			DebugHelper.WriteHeader("Starting SabMC.Notifo Process");

			if (args.Length >= 7)
			{
				SabNzbdJob job = new SabNzbdJob(args);
				NotifoPushNotification.Send(job);
			}
			else
			{
				Console.WriteLine("ERROR: no or to few parameters passed");
				Environment.Exit(1);
			}

			Console.WriteLine("== FINISHED SABMC.NOTIFO PROCESS ==");
		}
	}
}
