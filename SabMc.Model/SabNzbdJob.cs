namespace SabMc.Model
{
	using System.IO;
	using Enums;

	/// <summary>
	/// Model for a SabNZBD Job
	/// </summary>
	public class SabNzbdJob
	{
		private readonly DirectoryInfo directory;
		private MediaType mediaType = MediaType.Other;
		private SabNzbdStatus status;

		/// <summary>
		/// Constructs a SabNZBD object from the passed arguments of SabNZBD
		/// </summary>
		/// <param name="args">SabNZBD arguments</param>
		public SabNzbdJob(string[] args)
		{
			// Set Directory and File
			string path = args[0];
			directory = new DirectoryInfo(path);
			// Check Status
			status = GetStatusFromArgs(args);
		}

		/// <summary>
		/// Process the Job for a specific media type
		/// </summary>
		/// <param name="type">Mediatype of the job</param>
		public void Process(MediaType type)
		{
			mediaType = type;
		}

		/// <summary>
		/// Remove old/unused files and folder of the job
		/// </summary>
		public void CleanUp()
		{
			if (status != SabNzbdStatus.Ok)
				return;
			
			try
			{
				directory.Delete(true);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Strips the status from the arguments
		/// </summary>
		/// <param name="args">SabNZBD arguments</param>
		/// <returns>The SabNzbdStatus</returns>
		private static SabNzbdStatus GetStatusFromArgs(string[] args)
		{
			int lastIndex = args.Length - 1;
			string lastValue = args[lastIndex];
			switch (lastValue)
			{
				case "1":
					return SabNzbdStatus.FailedVerification;
				case "2":
					return SabNzbdStatus.FailedUnpacking;
				default:
					return SabNzbdStatus.Ok;
			}
		}

		#region Properties

		/// <summary>
		/// The Status
		/// </summary>
		public SabNzbdStatus Status
		{
			get { return status; }
			set { status = value; }
		}

		/// <summary>
		/// Fullname of the directory where the files are located
		/// </summary>
		public string FullFolderName
		{
			get { return directory.FullName; }
		}

		/// <summary>
		/// The foldername of the job
		/// </summary>
		public string FolderName
		{
			get { return directory.Name; }
		}

		/// <summary>
		/// The mediatype of the job
		/// </summary>
		public MediaType MediaType
		{
			get { return mediaType; }
		}

		#endregion
	}
}