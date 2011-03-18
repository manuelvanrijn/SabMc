namespace SabMc.Notifo
{
	using System;
	using Model;
	using Services.Config;
	using Services.Notifo;

	class Program
	{
		static void Main(string[] args)
		{
			if (ConfigReader.CheckConfig() == false)
			{
				Console.WriteLine("INFO: Config file created, please fill it :)");
				Environment.Exit(1);
			}
			Console.WriteLine("== STARTING SABMC.NOTIFO PROCESS ==");

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
